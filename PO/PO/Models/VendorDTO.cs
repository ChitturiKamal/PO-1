using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PO.Models
{   
    public class TEPOVendorMasterDTO
    {
        public int POVendorMasterId { get; set; }
        public string VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string ServiceTax { get; set; }
        public string RepresentName { get; set; }
        public string Relationship { get; set; }
        public string ContactNumber { get; set; }
        public string EmailID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
    }
    public class TEPOVendorMasterDetailDTO
    {
        public int POVendorDetailId { get; set; }
        public string VendorCode { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingPostalCode { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalCode { get; set; }
        public string RegionCode { get; set; }
        public string Country { get; set; }
        public string GSTApplicability { get; set; }
        public string GSTIN { get; set; }
        public Nullable<int> VendorAccountGroupId { get; set; }
        public Nullable<int> VendorCategoryId { get; set; }
        public Nullable<int> ScehmaGroupId { get; set; }
        public string GLAccountNo { get; set; }
        public string WithholdTaxType { get; set; }
        public string WithholdTaxCode { get; set; }
        public string WithholdApplicability { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<int> POVendorMasterId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedOn { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class VendorDTO
    {
        public int POVendorMasterId { get; set; }
        public int VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string Currency { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string ServiceTax { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public List<int> POVendorDetailId { get; set; }
        public List<string> VendorCode { get; set; }
        public List<string> BillingAddress { get; set; }
        public List<string> BillingCity { get; set; }
        public List<string> BillingPostalCode { get; set; }
        public List<string> ShippingAddress { get; set; }
        public List<string> ShippingCity { get; set; }
        public List<string> ShippingPostalCode { get; set; }
        public List<string> RegionId { get; set; }
        public List<int> Region { get; set; }
        public List<int> CountryId { get; set; }
        public List<int> Country { get; set; }
        public List<int> GSTApplicabilityId { get; set; }
        public List<int> GSTApplicability { get; set; }
        public List<string> GSTIN { get; set; }
        public List<int> VendorAccountGroupId { get; set; }
        public List<int> VendorAccountGroup { get; set; }
        public List<int> VendorCategoryId { get; set; }
        public List<int> VendorCategory { get; set; }
        public List<string> SchemaGroupId { get; set; }
        public List<int> SchemaGroup { get; set; }
        public List<int> GLAccountId { get; set; }
        public List<int> GLAccount { get; set; }
        public List<string> WithholdTaxTypeId { get; set; }
        public List<int> WithholdTaxType { get; set; }
        public List<string> WithholdTaxCodeId { get; set; }
        public List<int> WithholdTaxCode { get; set; }
        public List<string> WithholdApplicability { get; set; }
        public List<string> BankAccountName { get; set; }
        public List<string> BankAccountNumber { get; set; }
        public List<string> BankName { get; set; }
        public List<string> IFSCCode { get; set; }
        public List<string> RepresentName { get; set; }
        public List<string> Designation { get; set; }
        public List<string> RepresentContactNumber { get; set; }
        public List<string> RepresentEmailID { get; set; }
        public List<int> CancelledChequeRef { get; set; }
        public List<string> RepresentContactId { get; set; }
        public List<int> GSTNRegnCertificateRef { get; set; }
        public List<int> IncorporationCertificateRef { get; set; }
    }

    public class VendorMasterDto
    {
        public int POVendorMasterId { get; set; }
        public int VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string Currency { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string ServiceTax { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<VendorMasterDetailDto> VendorMasterDetails {get; set;}
    }

    public class VendorMasterDetailDto
    {
        public int POVendorDetailId { get; set; }
        public string VendorCode { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingPostalCode { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalCode { get; set; }
        public string RegionId { get; set; }
        public int Region { get; set; }
        public int CountryId { get; set; }
        public int Country { get; set; }
        public int GSTApplicabilityId { get; set; }
        public int GSTApplicability { get; set; }
        public string GSTIN { get; set; }
        public int VendorAccountGroupId { get; set; }
        public int VendorAccountGroup { get; set; }
        public int VendorCategoryId { get; set; }
        public int VendorCategory { get; set; }
        public string SchemaGroupId { get; set; }
        public int SchemaGroup { get; set; }
        public int GLAccountId { get; set; }
        public int GLAccount { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string RepresentName { get; set; }
        public string Designation { get; set; }
        public string RepresentContactNumber { get; set; }
        public string RepresentEmailID { get; set; }
        public int CancelledChequeRef { get; set; }
        public int RepresentContactId { get; set; }
        public int GSTNRegnCertificateRef { get; set; }
        public int IncorporationCertificateRef { get; set; }
        public List<VendorWithHoldApplicabilityDetailDto> WithHoldApplicabilityDetails { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class VendorWithHoldApplicabilityDetailDto
    {
        public int POVendorWithHoldApplicabilityDetailId { get; set; }
        public int WithHoldingTaxTypeId { get; set; }
        public int WithHoldingCodeId { get; set; }
        public bool WithHoldingApplicability { get; set; }
    }



    public class VendorMasterViewDto
    {
        public int POVendorMasterId { get; set; }
        public Nullable<int> VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string Currency { get; set; }
        public string PAN { get; set; }
        public string CIN { get; set; }
        public string ServiceTax { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<VendorMasterDetailViewDto> VendorMasterDetails { get; set; }
    }

    public class VendorMasterDetailViewDto
    {
        public int POVendorDetailId { get; set; }
        public string VendorCode { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingPostalCode { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingPostalCode { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string Region { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Country { get; set; }
        public Nullable<int> GSTApplicabilityId { get; set; }
        public string GSTApplicability { get; set; }
        public string GSTIN { get; set; }
        public Nullable<int> VendorAccountGroupId { get; set; }
        public string VendorAccountGroup { get; set; }
        public Nullable<int> VendorCategoryId { get; set; }
        public string VendorCategory { get; set; }
        public Nullable<int> SchemaGroupId { get; set; }
        public string SchemaGroup { get; set; }
        public Nullable<int> GLAccountId { get; set; }
        public string GLAccount { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<int> CancelledChequeRef { get; set; }
        public Nullable<int> GSTNRegnCertificateRef { get; set; }
        public Nullable<int> IncorporationCertificateRef { get; set; }
        public Nullable<int> RepresentContactId { get; set; }
        public string RepresentName { get; set; }
        public string RepresentContactNumber { get; set; }
        public string RepresentEmailID { get; set; }
        public string Designation { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<VendorWithHoldTaxDetailDto> WithHoldApplicabilityDetails { get; set; }
    }

    public class VendorWithHoldTaxDetailDto
    {
        public int POVendorWithHoldApplicabilityDetailId { get; set; }
        public Nullable<int> WithHoldingTaxTypeId { get; set; }
        public string WithHoldingTaxType { get; set; }
        public Nullable<int> WithHoldingCodeId { get; set; }
        public string WithHoldingCode { get; set; }
        public Nullable<bool> WithHoldingApplicability { get; set; }
    }

}