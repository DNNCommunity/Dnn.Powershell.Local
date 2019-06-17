using System.Management.Automation;

namespace Dnn.Powershell.Local
{
    public abstract class DNNCmdLet : PSCmdlet
    {
        public DNNContext Context
        {
            get
            {
                return DNNContext.Instance;
            }
        }

        public void WriteContextDetails()
        {
            WriteVerbose(string.Format("Connection String:    {0}", Context.ConnectionString));
            WriteVerbose(string.Format("Database Owner:       {0}", Context.DatabaseOwner));
            WriteVerbose(string.Format("Object Qualifier:     {0}", Context.ObjectQualifier));
            WriteVerbose(string.Format("Database:             {0}", Context.DatabaseName));
            WriteVerbose(string.Format("DNN Version:          {0}", Context.DNNVersion));
        }
    }
}
