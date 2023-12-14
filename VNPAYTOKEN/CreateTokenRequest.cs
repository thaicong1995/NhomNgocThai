namespace WebApi.VNPAYTOKEN
{
    public class CreateTokenRequest
    {
        public string vnp_version { get; set; }
        public string vnp_command { get; set; }
        public string vnp_tmn_code { get; set; }
        public string vnp_app_user_id { get; set; }
        public string vnp_bank_code { get; set; }
        public string vnp_locale { get; set; }
        public string vnp_card_type { get; set; }
        public string vnp_txn_ref { get; set; }
        public string vnp_txn_desc { get; set; }
        public string vnp_return_url { get; set; }
        public string vnp_cancel_url { get; set; }
        public string vnp_ip_addr { get; set; }
        public string vnp_create_date { get; set; }
        public string vnp_secure_hash { get; set; }
        public string vnp_base_url { get; set; }
       
    }
}
