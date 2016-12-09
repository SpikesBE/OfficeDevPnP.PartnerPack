using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using OfficeDevPnP.PartnerPack.Infrastructure;
using OfficeDevPnP.PartnerPack.Infrastructure.Jobs;

namespace OfficeDevPnP.PartnerPack.SiteProvisioning.Controllers
{
    [Authorize]
    [RoutePrefix("webapi")]    
    public class WebApiController : ApiController
    {
        #region Govarnance
        [HttpPost, Route("provsingle")]
        public Guid CreateSiteCollectionSingle([FromBody]SiteCollectionProvisioningJob value)
        {
            return this.EnqueueSiteCollectionProvisioningJob(value);
        }

        [HttpPost, Route("provmulti")]
        public Dictionary<Guid, SiteCollectionProvisioningJob> CreateSiteCollectionMulti([FromBody]List<SiteCollectionProvisioningJob> valueList)
        {
            Dictionary<Guid, SiteCollectionProvisioningJob> resultDictionary = new Dictionary<Guid, SiteCollectionProvisioningJob>();
            foreach (var value in valueList)
            {
                resultDictionary.Add(this.EnqueueSiteCollectionProvisioningJob(value), value);
            }
            return resultDictionary;
        }

        #region Private
        private Guid EnqueueSiteCollectionProvisioningJob(SiteCollectionProvisioningJob job)
        {
            //Enrich incoming job            
            job.Title = String.Format("Provisioning of Site Collection \"{1}\" with Template \"{0}\" by {2}",
                    job.ProvisioningTemplateUrl,
                    job.RelativeUrl,
                    job.Owner);

            var jobId = ProvisioningRepositoryFactory.Current.EnqueueProvisioningJob(job);

            return jobId;
        }
        #endregion
        #endregion


    }
}
