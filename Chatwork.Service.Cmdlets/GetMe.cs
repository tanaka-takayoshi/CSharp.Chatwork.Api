using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Chatwork.Service.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Me")]
    public class GetMe : Cmdlet
    {
        protected override void EndProcessing()
        {
            WriteObject("hogehoge!");
        }
    }
}
