using System;
using System.Drawing;
using Growl.Connector;
using Kata.Messages;

namespace Kata.Services
{
    class GrowlNotifier : Startable, Handles<ProjectBuilt>, Handles<TestRunComplete>
    {
        private Application _application = new Application("Code Kata");
        private GrowlConnector _connector = new GrowlConnector();

        public void Start()
        {
            _connector.Register(new Application("Code Kata"), new NotificationType[]
            {
                new NotificationType("Success"),
                new NotificationType("Failure")
            });
        }

        public void Handle(ProjectBuilt message)
        {
            if(message.Succeeded == false)
                _connector.Notify(new Notification("Code Kata", "Failure", "", "Fail", "Build Failed", Image.FromFile("exclamation.png"), false, Priority.Emergency, null));
        }

        public void Handle(TestRunComplete message)
        {
            if (message.Failed > 0)
            {
                var textMessage = String.Format("{0} out of {1} tests failed\n{2}", message.Failed, message.Total, String.Join("\n", message.Failures));
                _connector.Notify(new Notification("Code Kata", "Failure", "", "Fail", textMessage, Image.FromFile("exclamation.png"), false, Priority.Emergency, null));
            }
            else
            {
                _connector.Notify(new Notification("Code Kata", "Success", "", "Win", String.Format("{0} tests passed", message.Total), Image.FromFile("accept.png"), false, Priority.Normal, null));
            }
        }
    }
}
