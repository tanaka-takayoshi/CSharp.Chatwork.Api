using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chatwork.Service.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Greeting")]
    public class GetGreeting : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1)]
        [ValidateSet("さん", "様", "殿")]
        public string Title { get; set; }

        protected override void BeginProcessing()
        {
            WriteObject("Begin Hello World!");
        }

        protected override void ProcessRecord()
        {
            WriteObject(Name + " " + Title);
        }

        protected override void EndProcessing()
        {
            WriteObject("End Hello World!");
        }

        protected override void StopProcessing()
        {
            //これは出力されない
            WriteObject("Now stopping...");
        }
    }
}
