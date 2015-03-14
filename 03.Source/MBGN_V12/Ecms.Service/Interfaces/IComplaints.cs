using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz.Class;

namespace Ecms.Biz.Interfaces
{
    public interface IComplaints
    {
        List<Complaint> ComplaintGet(string id, string createdUser, string title, string status, ref string alParamsOutError);

        bool ComplaintDelete(string id, ref string alParamsOutError);

        Complaint ComplaintCreate(Complaint complaint, ref string alParamsOutError);

        Complaint ComplaintUpdate(Complaint complaint, ref string alParamsOutError);

        List<ComplaintDetailModel> ComplaintDetailGet(string id, string complaintId, string userCreatedId, ref string alParamsOutError);

        bool ComplaintDetailDelete(string id, ref string alParamsOutError);

        ComplaintDetail ComplaintDetailUpdate(ComplaintDetail complaintDetail, ref string alParamsOutError);

        ComplaintDetail ComplaintDetailCreate(ComplaintDetail complaintDetail, ref string alParamsOutError);
    }
}
