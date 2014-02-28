using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Chatwork.Service.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Greeting")]
    public class GetGreeting : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            WriteObject("Begin Hello!");
        }

        protected override void ProcessRecord()
        {
            WriteObject("World " + Name);
        }

        protected override void EndProcessing()
        {
            if (Stopping)
            {
                WriteObject("Stopped Hello.");
            }
            else
            {
                WriteObject("End Hello!");
            }
        }

        protected override void StopProcessing()
        {
            WriteObject("Now stopping...");
        }
    }
}
