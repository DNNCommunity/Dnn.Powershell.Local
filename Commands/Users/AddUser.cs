using System.Management.Automation;
using System.Data;
using System.Linq;
using Dnn.Powershell.Local.Dnn;

namespace Dnn.Powershell.Local.Commands.Users
{
    /// <summary>
    /// <para type="synopsis">Add user to DNN and to a portal</para>
    /// <para type="description">This command will create a user and add that user to the specified portal</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Add, Nouns.User)]
    public class AddUser : DNNCmdLet
    {
        /// <summary>
        /// <para type="description">Portal ID for the portal to add the user to</para>
        /// </summary>
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        /// <summary>
        /// <para type="description">Username to use. If not specified it will
        /// be auto generated based on first and lastname.</para>
        /// </summary>
        [Parameter(Position = 1)]
        public string Username { get; set; }

        /// <summary>
        /// <para type="description">Password for this user. If not specified
        /// it will be set to firstname+lastname.</para>
        /// </summary>
        [Parameter(Position = 2)]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">Email for the user. If not specified
        /// it will be set to firstname@lastname.name</para>
        /// </summary>
        [Parameter(Position = 3)]
        public string Email { get; set; }

        /// <summary>
        /// <para type="description">Displayname to use. If not specified
        /// it will be set to firstname lastname.</para>
        /// </summary>
        [Parameter(Position = 4)]
        public string DisplayName { get; set; }

        /// <summary>
        /// <para type="description">Firstname to use. If not specified
        /// a random name is used.</para>
        /// </summary>
        [Parameter(Position = 5)]
        public string Firstname { get; set; }

        /// <summary>
        /// <para type="description">Lastname to use. If not specified
        /// a random name is used.</para>
        /// </summary>
        [Parameter(Position = 6)]
        public string Lastname { get; set; }

        /// <summary>
        /// <para type="description">A comma separated list of rolenames to add the user
        /// to. If not specified the user will not be added to any roles other than
        /// the default (auto assign) roles for the portal.</para>
        /// </summary>
        [Parameter()]
        public string Roles { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(Firstname))
                Firstname = Common.Random.GetRandomFirstName();
            if (string.IsNullOrEmpty(Lastname))
                Lastname = Common.Random.GetRandomLastName();
            if (string.IsNullOrEmpty(DisplayName))
                DisplayName = string.Format("{0} {1}", Firstname, Lastname);
            if (string.IsNullOrEmpty(Email))
                Email = string.Format("{0}@{1}.name", Firstname, Lastname);
            if (string.IsNullOrEmpty(Username))
                Username = string.Format("{0}{1}{2}", Firstname.Substring(0, 1), Lastname, Common.Random.RandomNumber(0, 999));
            if (string.IsNullOrEmpty(Password))
                Password = Firstname + Lastname;

            WriteVerbose(string.Format("Creating User {0}", Username));

            int UserId = UserController.AddUser(Context, PortalId, Username, Password, Email, Firstname, Lastname, DisplayName);
            WriteVerbose(string.Format("Created User {0} with password {1}", DisplayName, Password));

            if (!string.IsNullOrEmpty(Roles))
            {
                foreach (var role in Roles.Replace(";", ",").Split(','))
                {
                    UserController.AddUserToRole(Context, PortalId, UserId, role);
                }
            }

            WriteObject(UserId);
        }
    }
}
