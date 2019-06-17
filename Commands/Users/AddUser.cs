using System.Management.Automation;
using System.Data;
using System.Linq;
using Dnn.Powershell.Local.Dnn;

namespace Dnn.Powershell.Local.Commands.Users
{
    [Cmdlet(VerbsCommon.Add, Nouns.User)]
    public class AddUser : DNNCmdLet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public int PortalId { get; set; }

        [Parameter(Position = 1)]
        public string Username { get; set; }

        [Parameter(Position = 2)]
        public string ClearTextPassword { get; set; }

        [Parameter(Position = 3)]
        public string Email { get; set; }

        [Parameter(Position = 4)]
        public string DisplayName { get; set; }

        [Parameter(Position = 5)]
        public string Firstname { get; set; }

        [Parameter(Position = 6)]
        public string Lastname { get; set; }

        [Parameter()]
        public string Roles { get; set; }

        [Parameter()]
        public int Minroles { get; set; } = -1;

        [Parameter()]
        public int Maxroles { get; set; } = -1;

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
                Username = string.Format("{0}{1}{2}", Firstname, Lastname, Common.Random.RandomNumber(0, 999));
            if (string.IsNullOrEmpty(ClearTextPassword))
                ClearTextPassword = Firstname + Lastname;

            WriteVerbose(string.Format("Creating User {0}", Username));

            int UserId = UserController.AddUser(Context, PortalId, Username, ClearTextPassword, Email, Firstname, Lastname, DisplayName);
            WriteVerbose(string.Format("Created User {0} with password {1}", DisplayName, ClearTextPassword));

            if (!string.IsNullOrEmpty(Roles))
            {
                System.Random rnd = new System.Random();
                IOrderedEnumerable<string> roleList = Roles.Replace(";", ",").Split(',').OrderBy(r => rnd.Next());
                if (Minroles == -1)
                    Minroles = roleList.Count();
                if (Maxroles == -1)
                    Maxroles = roleList.Count();
                int nrRolesToAdd = Common.Random.RandomNumber(Minroles, Maxroles);
                var loopTo = nrRolesToAdd;
                for (int i = 1; i <= loopTo; i++)
                    UserController.AddUserToRole(Context, PortalId, UserId, roleList.ElementAtOrDefault(i));
            }

            WriteObject(UserId);
        }
    }
}
