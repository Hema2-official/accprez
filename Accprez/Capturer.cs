using EventHook;
using System.Collections.Generic;
using System.Diagnostics;

namespace Accprez
{
    public class Capturer
    {
        #region ]  global variables  [
        Stopwatch stopwatch;
        EventHookFactory hookFactory;
        MouseWatcher mouseWatcher;
        KeyboardWatcher keyboardWatcher;
        private IList<inputEvent> inputEvents;
        #endregion

        #region -  public variables  -
        public IList<inputEvent> InputEvents
        {
            get
            {
                return inputEvents;
            }
        }

        public long TotalMillis
        {
            get
            {
                return stopwatch.ElapsedMilliseconds;
            }
        }
        #endregion

        #region >  Capturer()  <
        public Capturer()
        {
            // init
            stopwatch = new Stopwatch();
            hookFactory = new EventHookFactory();
            mouseWatcher = hookFactory.GetMouseWatcher();
            keyboardWatcher = hookFactory.GetKeyboardWatcher();
            inputEvents = new List<inputEvent>();

            // subscribe to input events (watching is not started anyways so it's better to do it here)
            mouseWatcher.OnMouseInput += handleMouseEvent;
            keyboardWatcher.OnKeyInput += handleKeyEvent;
        }
        #endregion

        #region !  Start() and Stop()  !
        public void Start()
        {
            // init list
            inputEvents.Clear();

            // watch mouse
            mouseWatcher.Start();

            // watch keyboard
            keyboardWatcher.Start();

            // reset and start stopwatch
            stopwatch.Restart();

        }

        public void Stop()
        {
            // unwatch mouse
            mouseWatcher.Stop();

            // unwatch keyboard
            keyboardWatcher.Stop();

            // stop stopwatch
            stopwatch.Stop();
        }
        #endregion

        #region }  inputEvent related classes  {
        public class inputEvent
        {
            public long millis;
        }

        public class mouseEvent : inputEvent
        {
            public mouseEvent(long millis, EventHook.Hooks.MouseMessages message, uint mouseData, EventHook.Hooks.Point point) {
                this.millis = millis;
                this.message = message;
                this.mouseData = mouseData;
                this.point = point;
            }
            EventHook.Hooks.MouseMessages message;
            uint mouseData;
            EventHook.Hooks.Point point;
        }
        
        public class keyEvent : inputEvent
        {
            public keyEvent(long millis, KeyData keyData) {
                this.millis = millis;
                this.keyData = keyData;
            }
            KeyData keyData;
        }
        #endregion

        #region )  input event handler functions  (
        private void handleMouseEvent(object s, MouseEventArgs e)
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            inputEvents.Add(new mouseEvent(currentTime, e.Message, e.MouseData, e.Point));
        }

        private void handleKeyEvent(object s, KeyInputEventArgs e)
        {
            long currentTime = stopwatch.ElapsedMilliseconds;
            inputEvents.Add(new keyEvent(currentTime, e.KeyData));
        }
        #endregion

    }
}
