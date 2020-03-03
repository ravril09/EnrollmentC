﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CST.Models
{
    class SchoolRequirementsController
    {
        crudFile cs = new crudFile();
        YearController yearController = new YearController();
        private int schoolYearId = 0;
        public SchoolRequirementsController()
        {
            schoolYearId = yearController.getSchoolYearId();
        }


        public void addSchoolRequirements(string typeStud,string name)
        {
            string sql = String.Format(@"INSERT INTO school_requirements (type_of_student,requirement_name,SY_ID) VALUES ('{0}','{1}',{2})",
                                       typeStud, name, schoolYearId);

            cs.ExecuteQuery(sql);
        }

        public void fillDataGridSchoolReq(ref DataGridView dg,int schoolId)
        {
            string sql = String.Format(@"SELECT req_id,type_of_student,requirement_name FROM school_requirements WHERE SY_ID = {0}", schoolId);

            cs.FillDataGrid(sql,ref dg);
        }

        public void updateSchoolRequirements(string typeStud,string name , int idReq)
        {
            string sql = String.Format(@"UPDATE school_requirements SET type_of_student = '{0}', requirement_name ='{1}' WHERE req_id = {2}",
                                        typeStud, name, idReq);

            cs.ExecuteQuery(sql);
        }

        public void removeSchoolReq(int idReq)
        {
            string sql = String.Format(@"DELETE FROM school_requirements WHERE req_id = {0}", idReq);

            cs.ExecuteQuery(sql);
        }

        public void searchGrid(string condition, string searchKeys, ref DataGridView dg,int schoolId)
        {
            string sql = String.Format(@"SELECT req_id,type_of_student,requirement_name FROM school_requirements WHERE SY_ID = {0} AND type_of_student = '{1}' AND requirement_name LIKE '%{2}%'", 
                                            schoolId,
                                            condition,
                                             searchKeys);

            cs.FillDataGrid(sql, ref dg);
        }
    }
}
