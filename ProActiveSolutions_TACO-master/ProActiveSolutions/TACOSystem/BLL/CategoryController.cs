using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TACOData.Entities;
using TACOData.Entities.POCOs;
using TACOSystem.DAL;

namespace TACOSystem.BLL
{
    [DataObject]
    public class CategoryController
    {
        /// <summary>
        /// This method returns a list of Category. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: February 28,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary> It shows list of active Category
        /// <returns> list of Category </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CategoryInformation> CategoryList()
        {
            using (var context = new TacoContext())
            {
                var result = from category in context.TB_TACO_Category
                             where category.ExpiryDate > DateTime.Now
                             select new CategoryInformation
                             {
                                 CategoryId = category.CategoryId,
                                 CategoryName = category.CategoryName
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a list of Category. It does not take any parameters.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: April 04, 2019
        /// Modified By: Prince Selhi
        /// Modified On: 
        /// </summary> It shows list of terminated Category
        /// <returns> list of Category </returns>

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CategoryInformation> CategoryListExpired()
        {
            using (var context = new TacoContext())
            {
                var result = from category in context.TB_TACO_Category
                             where category.ExpiryDate <= DateTime.Now
                             select new CategoryInformation
                             {
                                 CategoryId = category.CategoryId,
                                 CategoryName = category.CategoryName
                             };
                return result.ToList();
            }
        }



        /// <summary>
        /// This method returns Category DETAILS. parameters = Category ID.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: March 08,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Category details</returns>

        [DataObjectMethod(DataObjectMethodType.Select)]
        public CategoryInformation GetCategoryInformation(int CategoryId)
        {
            using (var context = new TacoContext())
            {
                var results = from category in context.TB_TACO_Category
                              where category.CategoryId == CategoryId
                              select new CategoryInformation
                              {
                                  CategoryId = category.CategoryId,
                                  CategoryName = category.CategoryName,
                                  CategoryDescription = category.CategoryDescription,
                                  StartDate = category.StartDate,
                                  ExpiryDate = category.ExpiryDate
                              };
                return results.Single();
            }
        }


        /// <summary>
        /// This method updates Category DETAILS. parameters = Category ID.
        /// Can be used to populate an ObjectDataSource.
        /// Created By: Prince Selhi
        /// Created On: March 08,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Updated Category details</returns>

        public void UpdateCategory(CategoryInformation updatedCategoryinfo)
        {
            using (var context = new TacoContext())
            {
                var categoryInfo = context.TB_TACO_Category.Find(updatedCategoryinfo.CategoryId);

                if (categoryInfo == null)
                {
                    throw new Exception("Category does not exist");
                }

                else if (updatedCategoryinfo.ExpiryDate < DateTime.Today)
                {
                    throw new Exception("Expiry date can not be before today.");
                }

                else if (updatedCategoryinfo.StartDate > updatedCategoryinfo.ExpiryDate)
                {
                    throw new Exception("Expiry date can not be before than start date.");
                }

                else
                {
                    categoryInfo.CategoryId = updatedCategoryinfo.CategoryId;
                    categoryInfo.CategoryName = updatedCategoryinfo.CategoryName;
                    categoryInfo.CategoryDescription = updatedCategoryinfo.CategoryDescription;
                    categoryInfo.StartDate = updatedCategoryinfo.StartDate;
                    categoryInfo.ExpiryDate = updatedCategoryinfo.ExpiryDate;
                    categoryInfo.ModifiedBy = updatedCategoryinfo.ModifiedBy;
                    categoryInfo.ModifiedOn = DateTime.Now;

                    context.Entry(categoryInfo).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// This method CREATE Category. parameters = Category ID.
        /// Created By: Prince Selhi
        /// Created On: March 08,2019
        /// Modified By: Prince Selhi
        /// Modified On:march 22, 2019
        /// </summary>
        /// <returns> Create Category</returns>
        public void CreateCategory(CategoryInformation newCategory)
        {
            using (var context = new TacoContext())
            {
                var newCategoryinfo = new TB_TACO_Category();

                if (newCategory.StartDate > newCategory.ExpiryDate)
                {
                    throw new Exception("Expiry date can not be before than start date");
                }

                else if (newCategory.StartDate < DateTime.Today)
                {
                    throw new Exception("Start date can not be before today.");
                }

                else
                {
                    newCategoryinfo.CategoryName = newCategory.CategoryName;
                    newCategoryinfo.CategoryDescription = newCategory.CategoryDescription;
                    newCategoryinfo.StartDate = newCategory.StartDate;
                    newCategoryinfo.ExpiryDate = newCategory.ExpiryDate;
                    newCategoryinfo.CreatedBy = newCategory.CreatedBy;
                    newCategoryinfo.CreatedOn = DateTime.Now;
                }

                context.TB_TACO_Category.Add(newCategoryinfo);
                context.SaveChanges();
            }
        }


        public void DeleteCategory(CategoryInformation DeletedCategoryInfo)
        {
            using (var context = new TacoContext())
            {
                var delCategory = context.TB_TACO_Category.Find(DeletedCategoryInfo.CategoryId);

                if (delCategory == null)
                {
                    throw new Exception("Category does not exist");
                }

                else
                {
                    delCategory.CategoryId = DeletedCategoryInfo.CategoryId;
                    delCategory.ExpiryDate = DateTime.Now;
                    delCategory.ModifiedBy = DeletedCategoryInfo.ModifiedBy;
                    delCategory.ModifiedOn = DateTime.Now;

                    context.Entry(delCategory).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public void ActivateCategory(CategoryInformation activatedcategoryinfo)
        {
            using (var context = new TacoContext())
            {
                var actCategory = context.TB_TACO_Category.Find(activatedcategoryinfo.CategoryId);

                if (actCategory == null)
                {
                    throw new Exception("Category does not exist");
                }

                else
                {
                    actCategory.CategoryId = activatedcategoryinfo.CategoryId;
                    actCategory.ExpiryDate = DateTime.Parse("December 31, 9999");
                    actCategory.ModifiedBy = activatedcategoryinfo.ModifiedBy;
                    actCategory.ModifiedOn = DateTime.Today;

                    context.Entry(actCategory).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

    }
}
