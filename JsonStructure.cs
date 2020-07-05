using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapper
{
    class JsonStructure
    {
        public class movie
        {
            public int id { get; set; }
            public string? url { get; set; }
            public string? name { get; set; }
            public string? type { get; set; }
            public string? language { get; set; }
            public string[]? genres { get; set; }
            public string? status { get; set; }
            public int? runtime { get; set; }
            public DateTime? premiered { get; set; }
            public string officialSite { get; set; }
            public STschedule? schedule { get; set; }
            public STrating? rating { get; set; }
            public int? weight { get; set; }
            public STnetwork? network { get; set; }
            public STnetwork? webChannel { get; set; }
            public STexternal? externals { get; set; }
            public STimage? image { get; set; }
            public string? summary { get; set; }
            public int? updated { get; set; }
            public STlink? _links { get; set; }
        }
        public class STschedule
        {
            public DateTime? time { get; set; }
            public string[]? days { get; set; }
        }
        public class STrating
        {
            public Single? Average { get; set; }
        }

        public class STcountry
        {
            public string? Name { get; set; }
            public string? Code { get; set; }
            public string? Timezone { get; set; }
        }

        public class STnetwork
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public STcountry? Country { get; set; }
        }
        public class STexternal
        {
            public int? Tvrage { get; set; }
            public int? Thetvdb { get; set; }
            public string? Imdb { get; set; }
        }
        public class STimage
        {
            public string? Medium { get; set; }
            public string? Original { get; set; }
        }
        public class STlink
        {
            public STself? Self { get; set; }
            public STpreviousepisode? Previousepisode { get; set; }
        }
        public class STself
        {
            public string? href { get; set; }
        }
        public class STpreviousepisode
        {
            public string? href { get; set; }
        }
        public class STcast
        {
            public STperson? Person { get; set; }
            public STcharacter? Character { get; set; }
            public bool? Self { get; set; }
            public bool? Voice { get; set; }
        }
        public class STperson
        {
            public int? Id { get; set; }
            public string? Url { get; set; }
            public string? Name { get; set; }
            public STcountry? Country { get; set; }
            public DateTime? Birthday { get; set; }
            public DateTime? Deathday { get; set; }
            public string? Gender { get; set; }
            public STimage? Image { get; set; }
            public STlink? _links { get; set; }
        }
        public class STcharacter
        {
            public int? Id { get; set; }
            public string? Url { get; set; }
            public string? Name { get; set; }
            public STimage? image { get; set; }
            public STlink? _links { get; set; }

        }
        public class ResponseShow
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public List<Actor>? Cast { get; set; }

        }
        public class Actor
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public string? Birthday { get; set; }
        }
        public class Pagenation
        {
            public int? PageId { get; set; }
            public List<ResponseShow> LsShow { get; set; }

        }
    }
}
