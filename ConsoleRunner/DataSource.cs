namespace ConsoleRunner
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using LocatorsRepo;
    using OfficeOpenXml;

    public class DataSource
    {
        public static bool GetDomainPath(out string path)
        {
            try
            {
                AppDomain domain = AppDomain.CurrentDomain;
                path = domain.BaseDirectory;
                if (path.Equals(string.Empty))
                {
                    throw new Exception("Domain path is empty");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
                
            }

            return true;
        }

        //build on top of epplus.dll
        public bool ExcelParser(int chunkSize, List<string> tags)
        {
            string path = string.Empty;
            try
            {
                if (GetDomainPath(out path))
                {
                    FileInfo file = new FileInfo(path + "SWTags.xlsx");
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                        
                        for(int i = sheet.Dimension.Start.Row + 1; i <= sheet.Dimension.End.Row && i <= chunkSize ;i++)
                        {
                            tags.Add(sheet.Cells[i,1].Value.ToString());
                        }
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return true;
        }
    }



}