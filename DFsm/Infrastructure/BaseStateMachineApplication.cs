using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public abstract class BaseStateMachineApplication
        : IStateMachineApplicationContext
    {
        static BaseStateMachineApplication()
        {
            GlobalStates = new ConcurrentDictionary<Type, IDictionary<Type, IStateMachineState>>();
        }
        protected BaseStateMachineApplication()
        {
            _context = new StateMachineContext(this);
            _localVariables = new Dictionary<string, object>();
            Extensions = new ExtensionManager();
            Initialize();
        }
        public Guid UId { get; private set; }
        public IExtensionManager Extensions { get; }
        private IStateMachineState _currentState;
        private IDictionary<string, object> _globalVariables;
        private readonly IStateMachineContext _context;
        private static readonly IDictionary<Type, IDictionary<Type, IStateMachineState>> GlobalStates;
        private IDictionary<Type, IStateMachineState> _states;
        private readonly IDictionary<string, object> _localVariables;
        IStateMachineState IStateMachineApplicationContext.CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
        IStateMachineContext IStateMachineApplicationContext.Context => _context;
        IExtensionManager IStateMachineApplicationContext.Extensions => Extensions;
        IDictionary<string, object> IStateMachineApplicationContext.GlobalVariables => _globalVariables;
        IDictionary<string, object> IStateMachineApplicationContext.LocalVariables => _localVariables;
        public void Load(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var bf = new BinaryFormatter();
                var globalVariables = (Dictionary<string, object>)bf.Deserialize(ms);
                _globalVariables = globalVariables;
                using (var br = new BinaryReader(ms))
                {
                    UId = br.ReadGuid();
                    var currentStateType = Type.GetType(br.ReadString());
                    _currentState = TryAddAndGetState(currentStateType);
                }
            }
        }
        public byte[] Save()
        {
            byte[] serialized;
            EnsureGlobalVariables();
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, _globalVariables);

                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteGuid(UId);
                    bw.Write(_currentState.GetType().AssemblyQualifiedName);
                }

                serialized = ms.ToArray();
            }

            return serialized;
        }
        public void Run()
        {
            EnsureGlobalVariables();
            _currentState.Run(_context);
        }
        public async Task RunAsync()
        {
            EnsureGlobalVariables();
            await _currentState.RunAsync(_context);
        }
        public void ResumeExternalActivity<TActivity>(object arg)
            where TActivity : ExternalCodeActivity
        {
            _currentState.ResumeExternalActivity<TActivity>(_context, arg);
        }
        public void ResumeExternalActivity<TActivity, TArg>(TArg arg)
            where TActivity : ExternalCodeActivity
        {
            _currentState.ResumeExternalActivity<TActivity, TArg>(_context, arg);
        }
        public async Task ResumeExternalActivityAsync<TActivity, TArg>(TArg arg)
            where TActivity : ExternalCodeActivity
        {
            await _currentState.ResumeExternalActivityAsync<TActivity, TArg>(_context, arg);
        }
        public async Task ResumeExternalActivityAsync<TActivity>(object arg)
            where TActivity : ExternalCodeActivity
        {
            await _currentState.ResumeExternalActivityAsync<TActivity>(_context, arg);
        }
        IStateMachineState TryAddAndGetState(Type stateType)
        {
            if (_states.TryGetValue(stateType, out IStateMachineState state))
                return state;

            _states[stateType] = state = (IStateMachineState)Activator.CreateInstance(stateType, false);

            return state;
        }
        TState TryAddAndGetState<TState>()
             where TState : IStateMachineState, new()
        {
            IStateMachineState state;
            var stateType = typeof(TState);

            if (_states.TryGetValue(stateType, out state))
                return (TState)state;

            _states[stateType] = state = new TState();

            return (TState)state;
        }
        TState IStateMachineApplicationContext.TryAddAndGetState<TState>()
            => TryAddAndGetState<TState>();
        void IStateMachineApplicationContext.SetStartupState(IStateMachineState state)
        {
            var transition = new StateMachineTransition();
            ((IStateMachineTransitionContext)transition).Destination = state;

            TryAddAndGetState<StartState>().AddTransition(transition);
        }

        void Initialize()
        {
            UId = Guid.NewGuid();

            lock (GlobalStates)
            {
                var thisType = GetType();
                if (!GlobalStates.TryGetValue(thisType, out IDictionary<Type, IStateMachineState> localStates))
                {
                    _states = GlobalStates[thisType] = new Dictionary<Type, IStateMachineState>();
                    TryAddAndGetState<StartState>();
                    TryAddAndGetState<FinalState>();
                    OnConfigure(new StateMachineBuilder(this));
                }
                else _states = localStates;
            }
            _currentState = TryAddAndGetState<StartState>();
        }
        protected abstract void InitializeGlobalVariables(IStateMachineContext context);
        protected abstract void OnConfigure(StateMachineBuilder builder);
        private void EnsureGlobalVariables()
        {
            if (_globalVariables == null)
            {
                _globalVariables = new Dictionary<string, object>();
                InitializeGlobalVariables(_context);
            }
        }
    }
}
