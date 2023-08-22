﻿using Graphene.Entities;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    public class Log : InstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Device")]
        [ForeignKey(nameof(Device))]
        [JsonConverter(typeof(GuidConverter<Device>))]
        public virtual int? DeviceId { get; set; }
    }
}
