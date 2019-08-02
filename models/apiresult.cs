using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot.models
{
    public class apiresult
    {
        public List<Result> result { get; set; }
    }
    public class Result
    {
        public string short_description { get; set; }
        public string roles { get; set; }
        public object wiki { get; set; }
        public string direct { get; set; }
        public string rating { get; set; }
        public string description { get; set; }
        public object source { get; set; }
        public string sys_updated_on { get; set; }
        public string disable_suggesting { get; set; }
        public string sys_class_name { get; set; }
        public string number { get; set; }
        public string sys_id { get; set; }
        public string use_count { get; set; }
        public string sys_updated_by { get; set; }
        public string flagged { get; set; }
        public string disable_commenting { get; set; }
        public string sys_created_on { get; set; }
      //  public SysDomain sys_domain { get; set; }
        public string valid_to { get; set; }
        public string retired { get; set; }
        public string workflow_state { get; set; }
        public string text { get; set; }
        public string sys_created_by { get; set; }
        public string display_attachments { get; set; }
        public string image { get; set; }
        public string sys_view_count { get; set; }
        public string article_type { get; set; }
        public string cmdb_ci { get; set; }
   //     public Author author { get; set; }
        public string can_read_user_criteria { get; set; }
        public string sys_mod_count { get; set; }
        public string active { get; set; }
        public string cannot_read_user_criteria { get; set; }
        public string published { get; set; }
        public string sys_domain_path { get; set; }
        public string sys_tags { get; set; }
    //    public KbKnowledgeBase kb_knowledge_base { get; set; }
        public string meta { get; set; }
        public string topic { get; set; }
        public string category { get; set; }
    //    public KbCategory kb_category { get; set; }
    }
}
