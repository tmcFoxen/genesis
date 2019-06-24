﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Genesis;
using Genesis.Output;

namespace Genesis.Output.MvcController
{
    public class MvcControllerGenerator : OutputExecutor
    {
        public override string CommandText => "aspnet-mvc-con";
        public override string Description => "Generates an Asp.Net Controller based on an entity.";
        public override string FriendlyName => "Asp.Net MvcController OutputExecutor";

        public MvcControllerConfig Config { get; set; } 

        protected override void OnInitilized()
        {
            Config = (MvcControllerConfig)Configuration;
        }

        public override async Task<ITaskResult> Execute(GenesisContext genesis, string[] args)
        {
            var result = new OutputTaskResult(); //overridden just to loop over all the graphs

            foreach(var obj in genesis.Objects)
            {
                await ExecuteGraph(obj);
            }

            return result;
        }

        public Task ExecuteGraph(ObjectGraph objectGraph)
        {
            var tmp = this.Template;

            if (Directory.Exists(OutputPath))
                Directory.CreateDirectory(OutputPath);
            
            var output = tmp.Raw.Replace(Tokens.Namespace, Config.Namespace)    //TODO: Templating engine? / razor etc would be cool ..|., T4 
                                .Replace(Tokens.ObjectName, objectGraph.Name.ToSingular());

            var subPath = Path.Combine(OutputPath, "MvcControllers");

            if (!Directory.Exists(subPath))
                Directory.CreateDirectory(subPath);

            File.WriteAllText(Path.Combine(subPath, objectGraph.Name.ToSingular() + "Controller.cs"), output);

            return Task.CompletedTask;
        }
    }
}
