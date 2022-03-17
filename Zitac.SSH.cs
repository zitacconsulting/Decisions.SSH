using DecisionsFramework.Design.Flow;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Flow.Service.Debugging.DebugData;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.Design.Properties.Attributes;
using Renci.SshNet;
using System.Runtime.Serialization;


namespace zitac.SSH
{
    [Writable]
    [DataContract]
    public class SSHCredentials : IDebuggerJsonProvider
    {

        [WritableValue]
        private string userName;

        [WritableValue]
        private string userPassword;

        [PropertyClassification(new[] { "SSH Settings" }, 1)]
        [DataMember]
        public string User
        {
            get { return userName; }
            set { userName = value; }
        }


        [PropertyClassification(new[] { "SSH Settings" }, 2)]
        [PasswordText]
        [DataMember]
        public string Password
        {
            get { return userPassword; }
            set { userPassword = value; }
        }


        public object GetJsonDebugView()
        {
            return new
            {
                User = this.User,
                Password = "********"
            };
        }
    }


    [AutoRegisterMethodsOnClass(true, "Integration", "SSH")]
        public class SSHSteps
        {
            public string ExecuteSSHCommand(SSHCredentials Credentials, string Host, string Command, int Port = 22)
            {

                if (Port == 0) {
                   Port = 22;
                }

                var sshClient = new SshClient(Host, Port, Credentials.User, Credentials.Password);

                using (sshClient)
                {
                    sshClient.Connect();
                    var Result = sshClient.RunCommand(Command);
                    sshClient.Disconnect();
                    sshClient.Dispose();
                    return Result.Result;
                }
            }
        }
}
