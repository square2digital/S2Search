using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response.Facets
{
    public class FacetResponse
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits hits { get; set; }
        public Aggregations aggregations { get; set; }
    }

    public class Aggregations
    {
        public GroupByFuelType group_by_fuelType { get; set; }
        public GroupByBodyStyle group_by_bodyStyle { get; set; }
        public GroupByDoors group_by_doors { get; set; }
        public GroupByPriceInterval group_by_price_interval { get; set; }
        public GroupByTransmission group_by_transmission { get; set; }
        public GroupByMake group_by_make { get; set; }
        public GroupByPriceRange group_by_price_range { get; set; }
        public GroupByMonthlyPriceRange group_by_monthlyPrice_range { get; set; }
        public GroupByVariant group_by_variant { get; set; }
        public GroupByMonthlyPriceInterval group_by_monthlyPrice_interval { get; set; }
        public GroupByYear group_by_year { get; set; }
        public GroupByModel group_by_model { get; set; }
        public GroupByColour group_by_colour { get; set; }
        public GroupByEngineSize group_by_engineSize { get; set; }
    }

    public class Buckets
    {
        public List<Bucket> buckets { get; set; }
    }

    public class FacetElasticGroup : Buckets
    {
        public FacetElasticGroup() { }

        public FacetElasticGroup(string name) : this()
        {
            this.name = name;
        }

        public string name { get; set; }
        public int doc_count_error_upper_bound { get; set; }
        public int sum_other_doc_count { get; set; }
    }

    public class Bucket
    {
        public string key { get; set; }
        public int doc_count { get; set; }
        public double to { get; set; }
        public double? from { get; set; }
    }

    public class GroupByBodyStyle : FacetElasticGroup
    {

    }

    public class GroupByColour : FacetElasticGroup
    {

    }

    public class GroupByDoors : FacetElasticGroup
    {

    }

    public class GroupByEngineSize : FacetElasticGroup
    {

    }

    public class GroupByFuelType : FacetElasticGroup
    {

    }

    public class GroupByMake : FacetElasticGroup
    {

    }

    public class GroupByModel : FacetElasticGroup
    {

    }

    public class GroupByMonthlyPriceInterval : Buckets
    {

    }

    public class GroupByMonthlyPriceRange : Buckets
    {

    }

    public class GroupByPriceInterval : Buckets
    {

    }

    public class GroupByPriceRange : Buckets
    {

    }

    public class GroupByTransmission : FacetElasticGroup
    {

    }

    public class GroupByVariant : FacetElasticGroup
    {

    }

    public class GroupByYear : FacetElasticGroup
    {

    }

    public class Hits
    {
        public Total total { get; set; }
        public object max_score { get; set; }
        public List<object> hits { get; set; }
    }

    public class Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int skipped { get; set; }
        public int failed { get; set; }
    }

    public class Total
    {
        public int value { get; set; }
        public string relation { get; set; }
    }
}