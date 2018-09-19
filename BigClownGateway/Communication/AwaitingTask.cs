using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adastra.BigClownGateway
{
    internal class AwaitingTask
    {
        public AwaitingTask(string expectedResponse, int timeout)
        {
            ExpectedResponse = expectedResponse;
            _timeout = timeout;
        }

        /// <summary>
        /// beginning part of topic of response message
        /// </summary>
        public string ExpectedResponse { get; private set; }

        /// <summary>
        /// received response message
        /// </summary>
        public MqttMessage Response { get; set; }

        /// <summary>
        /// response was not received before defined timeout
        /// </summary>
        public bool TimeoutExpired { get; private set; }

        /// <summary>
        /// semaphore
        /// </summary>
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);


        /// <summary>
        /// wait until set is called by callback process
        /// </summary>
        /// <returns></returns>
        public bool WaitOne()
        {
            // prepare timeout for interrupt long running requests
            if (_timeout != Timeout.Infinite)
                _timer = new Timer(InterruptCallback, null, _timeout, Timeout.Infinite);

            return _resetEvent.WaitOne();
        }

        /// <summary>
        /// release main thread waiting on WaitOne step
        /// </summary>
        /// <returns></returns>
        public bool Set()
        {
            lock (_resetEvent)
            {
                return _resetEvent.Set();
            }
        }

        private int _timeout = Timeout.Infinite;

        private Timer _timer;

        void InterruptCallback(object state)
        {
            lock (_resetEvent)
            {
                TimeoutExpired = true;
                _resetEvent.Set();
            }
        }
    }
}
