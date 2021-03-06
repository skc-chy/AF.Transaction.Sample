﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AF.Transaction.Common;
using Architecture.Foundation.DataAccessor;
using Architecture.Foundation.DataAccessor.SqlClient;
using AF.Transaction.Entity.WCFTransactionDemo;

namespace AF.Transaction.Data.WCFTransactionDemo
{
    [AFDataStore("AF")]
    public class WCFTransactionDemoDAL : AFDataStoreAccessor, IWCFTransactionDemoDAL
    {
        public Result AddEmployee(WCFTransactionEntity employeeData)
        {
            var result = new Result() { IsValid = false };

            StoreProcedureCommand procedure = CreateProcedureCommand("dbo.InsertEmployee");
            procedure.AppendGuid("EmpID", employeeData.EmpID);
            procedure.AppendNVarChar("Name", employeeData.Name);
            procedure.AppendNVarChar("Address", employeeData.Address);
            procedure.AppendNVarChar("EMail", employeeData.EMail);
            procedure.AppendNVarChar("Phone", employeeData.Phone);

            int resultValue = ExecuteCommand(procedure);

            if (resultValue == 0)
            {
                result.IsValid = true;
                result.Message = new List<string> { "Employee added successfully" };
            }

            return result;
        }

        public Result UpdateEmployee(WCFTransactionEntity employeeData)
        {
            var result = new Result() { IsValid = false };

            StoreProcedureCommand procedure = CreateProcedureCommand("dbo.UpdateEmployee");
            procedure.AppendGuid("EmpID", employeeData.EmpID);
            procedure.AppendNVarChar("Address", employeeData.Address);
            procedure.AppendNVarChar("EMail", employeeData.EMail);
            procedure.AppendNVarChar("Phone", employeeData.Phone);

            int resultValue = ExecuteCommand(procedure);

            if (resultValue == 0)
            {
                result.IsValid = true;
                result.Message = new List<string> { "Employee updated successfully" };
            }

            return result;
        }

        public Result DeleteEmployee(Guid empID)
        {
            var result = new Result() { IsValid = false };

            StoreProcedureCommand procedure = CreateProcedureCommand("dbo.DeleteEmployee");
            procedure.AppendGuid("EmpID", empID);

            int resultValue = ExecuteCommand(procedure);

            if (resultValue == 0)
            {
                result.IsValid = true;
                result.Message = new List<string> { "Employee deleted successfully" };
            }

            return result;
        }

        public List<WCFTransactionEntity> GetEmployeeList()
        {
            var empList = new List<WCFTransactionEntity>();

            SqlDataReader reader = null;

            try
            {
                StoreProcedureCommand procedure = CreateProcedureCommand("dbo.GetEmployee");
                reader = ExecuteCommandAndReturnDataReader(procedure);

                while (reader.Read())
                    empList.Add(new WCFTransactionEntity { EmpID = new Guid(reader["EmpID"].ToString()), Name = reader["Name"].ToString(), Address = reader["Address"].ToString(), EMail = reader["EMail"].ToString(), Phone = reader["Phone"].ToString() });

                reader.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
                throw ex;
            }

            return empList;
        }
    }
}
