// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.BotBuilderSamples
{
    public class BookingDetails
    {
        public string Destination { get; set; }

        public string Origin { get; set; }

        public string TravelDate { get; set; }

        public string Short_desc { get; set; }

        public string Descrip { get; set; }

        public string Priority { get; set; }

        public string Incident_No { get; set; }

        //public string PriorityCode { get {

        //        if (!(Priority.Equals(null)))
        //        {
        //            if (Priority.Equals("High"))
        //            {
        //                return "1";
        //            }
        //            if (Priority.Equals("Medium"))
        //            {
        //                return "2";
        //            }
        //            if (Priority.Equals("Low"))
        //            {
        //                return "3";
        //            }
        //        }
        //        return "3";
        //    }
        //}

        public string Create_incident { get; set; }

        public string None { get; set; }
        public string Incident_status { get; set; }

        internal string GetPrioritycode(string prio)
        {
            if (!(prio.Equals(null)))
            {
                if (prio.Equals("High"))
                {
                    return "1";
                }
                if (prio.Equals("Medium"))
                {
                    return "2";
                }
                if (prio.Equals("Low"))
                {
                    return "3";
                }
            }
            return "3";
        }
    }
    }
