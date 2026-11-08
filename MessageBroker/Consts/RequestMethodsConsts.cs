using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Consts
{
    public static class RequestMethodsConsts
    {
        //public const List<string> consts { get;set } =new List() {
        //    "enqueue",
        //    "dequeue",
        //    "createqueue"

        //};

        public static readonly List<string> Consts = new()
        {
            "enqueue",
            "dequeue",
            "createqueue"
        };



    }
}