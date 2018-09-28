﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEComplaintsManagementAPI.SAPResponse {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://total-environment/PI/PORelease1", ConfigurationName="SAPResponse.PORelease1_Out")]
    public interface PORelease1_Out {
        
        // CODEGEN: Generating message contract since the operation PORelease1_Out is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse PORelease1_Out(TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse> PORelease1_OutAsync(TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://total-environment/PI/PORelease1")]
    public partial class POReleaseReq1Item : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string pURCHASEORDERField;
        
        private string rEL_CODEField;
        
        private string rEMARKSField;
        
        private string aPPROVERField;
        
        private string fUGUE_IDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string PURCHASEORDER {
            get {
                return this.pURCHASEORDERField;
            }
            set {
                this.pURCHASEORDERField = value;
                this.RaisePropertyChanged("PURCHASEORDER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string REL_CODE {
            get {
                return this.rEL_CODEField;
            }
            set {
                this.rEL_CODEField = value;
                this.RaisePropertyChanged("REL_CODE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string REMARKS {
            get {
                return this.rEMARKSField;
            }
            set {
                this.rEMARKSField = value;
                this.RaisePropertyChanged("REMARKS");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string APPROVER {
            get {
                return this.aPPROVERField;
            }
            set {
                this.aPPROVERField = value;
                this.RaisePropertyChanged("APPROVER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string FUGUE_ID {
            get {
                return this.fUGUE_IDField;
            }
            set {
                this.fUGUE_IDField = value;
                this.RaisePropertyChanged("FUGUE_ID");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://total-environment/PI/PORelease1")]
    public partial class POReleaseRes1Item : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string pO_NUMBERField;
        
        private string rETCODEField;
        
        private string mESSAGEField;
        
        private string fUGUE_IDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string PO_NUMBER {
            get {
                return this.pO_NUMBERField;
            }
            set {
                this.pO_NUMBERField = value;
                this.RaisePropertyChanged("PO_NUMBER");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string RETCODE {
            get {
                return this.rETCODEField;
            }
            set {
                this.rETCODEField = value;
                this.RaisePropertyChanged("RETCODE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MESSAGE {
            get {
                return this.mESSAGEField;
            }
            set {
                this.mESSAGEField = value;
                this.RaisePropertyChanged("MESSAGE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string FUGUE_ID {
            get {
                return this.fUGUE_IDField;
            }
            set {
                this.fUGUE_IDField = value;
                this.RaisePropertyChanged("FUGUE_ID");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PORelease1_OutRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://total-environment/PI/PORelease1", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TEComplaintsManagementAPI.SAPResponse.POReleaseReq1Item[] POReleaseReq1;
        
        public PORelease1_OutRequest() {
        }
        
        public PORelease1_OutRequest(TEComplaintsManagementAPI.SAPResponse.POReleaseReq1Item[] POReleaseReq1) {
            this.POReleaseReq1 = POReleaseReq1;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PORelease1_OutResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://total-environment/PI/PORelease1", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public TEComplaintsManagementAPI.SAPResponse.POReleaseRes1Item[] POReleaseRes1;
        
        public PORelease1_OutResponse() {
        }
        
        public PORelease1_OutResponse(TEComplaintsManagementAPI.SAPResponse.POReleaseRes1Item[] POReleaseRes1) {
            this.POReleaseRes1 = POReleaseRes1;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PORelease1_OutChannel : TEComplaintsManagementAPI.SAPResponse.PORelease1_Out, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PORelease1_OutClient : System.ServiceModel.ClientBase<TEComplaintsManagementAPI.SAPResponse.PORelease1_Out>, TEComplaintsManagementAPI.SAPResponse.PORelease1_Out {
        
        public PORelease1_OutClient() {
        }
        
        public PORelease1_OutClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PORelease1_OutClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }   
        
        public PORelease1_OutClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PORelease1_OutClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse TEComplaintsManagementAPI.SAPResponse.PORelease1_Out.PORelease1_Out(TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest request) {
            return base.Channel.PORelease1_Out(request);
        }
        
        public TEComplaintsManagementAPI.SAPResponse.POReleaseRes1Item[] PORelease1_Out(TEComplaintsManagementAPI.SAPResponse.POReleaseReq1Item[] POReleaseReq1) {
            TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest inValue = new TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest();
            inValue.POReleaseReq1 = POReleaseReq1;
            TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse retVal = ((TEComplaintsManagementAPI.SAPResponse.PORelease1_Out)(this)).PORelease1_Out(inValue);
            return retVal.POReleaseRes1;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse> TEComplaintsManagementAPI.SAPResponse.PORelease1_Out.PORelease1_OutAsync(TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest request) {
            return base.Channel.PORelease1_OutAsync(request);
        }
        
        public System.Threading.Tasks.Task<TEComplaintsManagementAPI.SAPResponse.PORelease1_OutResponse> PORelease1_OutAsync(TEComplaintsManagementAPI.SAPResponse.POReleaseReq1Item[] POReleaseReq1) {
            TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest inValue = new TEComplaintsManagementAPI.SAPResponse.PORelease1_OutRequest();
            inValue.POReleaseReq1 = POReleaseReq1;
            return ((TEComplaintsManagementAPI.SAPResponse.PORelease1_Out)(this)).PORelease1_OutAsync(inValue);
        }
    }
}
