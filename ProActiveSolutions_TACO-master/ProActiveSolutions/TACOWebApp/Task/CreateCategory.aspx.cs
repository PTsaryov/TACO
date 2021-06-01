using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TACOData.Entities.POCOs;
using TACOSystem.BLL;
using TACOSystem.BLL.Security;

namespace TACOWebApp.Task
{
    public partial class CreateCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var windowsAccountName = User.Identity.Name.Split('\\').Last();
            if (Request.IsAuthenticated && (User.Identity.Name.Split('\\').Last() == SecurityController.FindAccount(windowsAccountName).DisplayName && SecurityController.FindAccount(windowsAccountName).SecurityRoleName == "GlobalAdmin"))
            {
                //you can create a generic lable, to have the role you are currently in displayed for you
                //but in production it should be empty if you are not planing displain the role or 
                //employeeName/ID
               // Label1.Text = "You're the Boss " + User.Identity.Name.Split('\\').Last() + " your role is: " + SecurityController.FindAccount(windowsAccountName).SecurityRoleName;
            }
            else
            {
                all_content.Visible = false;
            }
            TextBoxCategoryId.ReadOnly = true;
            MessageUserControl.Visible = true;
            TextBoxStartDate.Enabled = true;
        }

        protected void ButtonLookupCategory_Click(object sender, EventArgs e)
        {
            if (DropDownCategory.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select Category first to lookup");
                TextBoxCategoryId.Text = "";
                TextBoxCategoryName.Text = "";
                TextBoxCategoryDescription.Text = "";
                TextBoxExpiryDate.Text = "";
                TextBoxStartDate.Text = "";
                ButtonUpdateCategory.Visible = false;
                ButtonDeleteCategory.Visible = false;
                ButtonCreateCategory.Visible = true;
            }
            else
            {
                var controller = new CategoryController();
                var categoryInfo = controller.GetCategoryInformation(int.Parse(DropDownCategory.SelectedValue));

                TextBoxCategoryId.Text = categoryInfo.CategoryId.ToString();
                TextBoxCategoryName.Text = categoryInfo.CategoryName;
                TextBoxCategoryDescription.Text = categoryInfo.CategoryDescription;
                TextBoxStartDate.Text = categoryInfo.StartDate.ToLongDateString();
                TextBoxExpiryDate.Text = categoryInfo.ExpiryDate.ToLongDateString();

                TextBoxStartDate.Enabled = false;

                if (CheckboxExpired.Checked == true)
                {
                    ButtonUpdateCategory.Visible = false;
                    ButtonDeleteCategory.Visible = false;
                    ButtonActivateCategory.Visible = true;
                    TextBoxCategoryName.Enabled = false;
                    TextBoxCategoryDescription.Enabled = false;
                    TextBoxExpiryDate.Enabled = false;
                }
                else
                {
                    ButtonUpdateCategory.Visible = true;
                    ButtonDeleteCategory.Visible = true;
                }
                ButtonCreateCategory.Visible = false;
                MessageUserControl.Visible = false;
            }
        }

        protected void ButtonCreateCategory_Click(object sender, EventArgs e)
        {

            MessageUserControl.TryRun(() =>
            {
                CategoryInformation newCategory = new CategoryInformation();
                if (TextBoxCategoryName.Text == null)
                {
                    throw new Exception("Category name cannot be empty.");
                }      
                
                else if (TextBoxCategoryDescription.Text == null)
                {
                    throw new Exception("Category Description cannot be empty.");
                } 
                else
                {
                    DateTime convertedDate;
                    newCategory.CategoryName = TextBoxCategoryName.Text;
                    newCategory.CategoryDescription = TextBoxCategoryDescription.Text;

                    if (DateTime.TryParse(TextBoxStartDate.Text, out convertedDate))
                    {
                        newCategory.StartDate = convertedDate;
                    }
                    else
                    {
                        throw new Exception("Please input Start Date in MM-DD-YYYY format (example: 04-26-2019) or choose from Date Picker.");
                    }

                    if (DateTime.TryParse(TextBoxExpiryDate.Text, out convertedDate))
                    {
                        newCategory.ExpiryDate = convertedDate;
                    }
                    else if (DateTime.TryParse("December 31, 9999", out convertedDate))
                    {
                        newCategory.ExpiryDate = convertedDate;
                    }

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    newCategory.CreatedBy = SecurityController.FindAccount(windowsAccountName).AccountID;
                }

                var controller = new CategoryController();
                controller.CreateCategory(newCategory);

                TextBoxCategoryId.Text = "";
                TextBoxCategoryName.Text = "";
                TextBoxCategoryDescription.Text = "";
                TextBoxExpiryDate.Text = "";
                TextBoxStartDate.Text = "";

                DropDownCategory.Items.Clear();
                DropDownCategory.DataBind();
                DropDownCategory.Items.Insert(0, new ListItem("Choose a Category", ""));
                DropDownCategory.SelectedIndex = -1;

            }, "Success", "Category : " +TextBoxCategoryName.Text+ " has been created.");
        }

        protected void ButtonUpdateCategory_Click(object sender, EventArgs e)
        {
            TextBoxStartDate.Enabled = false;
            MessageUserControl.TryRun(() =>
            {
                CategoryInformation updatedCategoryinfo = new CategoryInformation();
                if (TextBoxCategoryName.Text == null)
                {
                    throw new Exception(" Category name cannot be empty.");
                }

                else if (TextBoxCategoryDescription.Text == null)
                {
                    throw new Exception(" Category description cannot be empty.");
                }
                else if (TextBoxCategoryDescription.Text.Length > 250)
                {
                    throw new Exception(" Category description cannot be longer than 250 words.");
                }
                else
                {
                    updatedCategoryinfo.CategoryId = int.Parse(TextBoxCategoryId.Text);
                    updatedCategoryinfo.CategoryName = TextBoxCategoryName.Text;
                    updatedCategoryinfo.CategoryDescription = TextBoxCategoryDescription.Text;
                    updatedCategoryinfo.StartDate = Convert.ToDateTime(TextBoxStartDate.Text);
                    updatedCategoryinfo.ExpiryDate = Convert.ToDateTime(TextBoxExpiryDate.Text);

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    updatedCategoryinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;
                }
                var controller = new CategoryController();
                controller.UpdateCategory(updatedCategoryinfo);

                DropDownCategory.Items.Clear();
                DropDownCategory.DataBind();
                DropDownCategory.Items.Insert(0, new ListItem("Choose a Category", ""));
                DropDownCategory.SelectedIndex = -1;

            }, "Success", "Category : "+ TextBoxCategoryName.Text +" has been updated.");
        }

        protected void ButtonCancelCategory_Click(object sender, EventArgs e)
        {
            TextBoxCategoryId.Text = "";
            TextBoxCategoryName.Text = "";
            TextBoxCategoryDescription.Text = "";
            TextBoxExpiryDate.Text = "";
            TextBoxStartDate.Text = "";
            DropDownCategory.SelectedIndex = 0;

            ButtonCreateCategory.Visible = true;
            ButtonUpdateCategory.Visible = false;
            ButtonDeleteCategory.Visible = false;
            MessageUserControl.Visible = false;
            Response.Redirect(Request.RawUrl);
        }

        protected void ButtonDeleteCategory_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                CategoryInformation DeletedCategoryInfo = new CategoryInformation();
                if (TextBoxCategoryId.Text == null)
                {
                    throw new Exception("Please select a category to delete");
                }
                else
                {
                    DeletedCategoryInfo.CategoryId = int.Parse(TextBoxCategoryId.Text);
                    DeletedCategoryInfo.ExpiryDate = DateTime.Today;

                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    DeletedCategoryInfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxCategoryId.Text = "";
                    TextBoxCategoryName.Text = "";
                    TextBoxCategoryDescription.Text = "";
                    TextBoxExpiryDate.Text = "";
                    TextBoxStartDate.Text = "";                                     
                }
                var controller = new CategoryController();
                controller.DeleteCategory(DeletedCategoryInfo);
                
                DropDownCategory.Items.Clear();
                DropDownCategory.DataBind();
                DropDownCategory.Items.Insert(0, new ListItem("Choose a Category", ""));
                DropDownCategory.SelectedIndex = -1;
            }, "Success", "Category : "+TextBoxCategoryName.Text+" has been terminated.");
                        
            ButtonCreateCategory.Visible = true;
            ButtonUpdateCategory.Visible = false;
            ButtonDeleteCategory.Visible = false;
        }


        protected void CheckboxExpired_CheckedChanged(object sender, EventArgs e)
        {
            TextBoxCategoryId.Text = "";
            TextBoxCategoryName.Text = "";
            TextBoxCategoryDescription.Text = "";
            TextBoxExpiryDate.Text = "";
            TextBoxStartDate.Text = "";
            DropDownCategory.SelectedIndex = 0;

            ButtonCreateCategory.Visible = true;
            ButtonUpdateCategory.Visible = false;
            ButtonDeleteCategory.Visible = false;
            MessageUserControl.Visible = false;

            if (CheckboxExpired.Checked == true)
            {
                DropDownCategory.Items.Clear();
                CategorylistDataSource.SelectMethod = "CategoryListExpired";

                DropDownCategory.Items.Insert(0, new ListItem("Choose a Category", ""));
                DropDownCategory.SelectedIndex = 0;
                DropDownCategory.DataBind();
                ButtonUpdateCategory.Visible = false;
                ButtonDeleteCategory.Visible = false;
                ButtonCreateCategory.Visible = false;
            }
            else
            {

                DropDownCategory.Items.Clear();
                CategorylistDataSource.SelectMethod = "CategoryList";

                DropDownCategory.Items.Insert(0, new ListItem("Choose a Category", ""));
                DropDownCategory.SelectedIndex = 0;
                DropDownCategory.DataBind();

                ButtonActivateCategory.Visible = false;
                TextBoxCategoryName.Enabled = true;
                TextBoxCategoryDescription.Enabled = true;
                TextBoxExpiryDate.Enabled = true;
                TextBoxStartDate.Enabled = true;
            }
        }

        protected void ButtonActivateCategory_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                CategoryInformation activatedcategoryinfo = new CategoryInformation();
                if (TextBoxCategoryId.Text == null)
                {
                    throw new Exception("Please select a Category to Activate");
                }
                else
                {
                    activatedcategoryinfo.CategoryId = int.Parse(TextBoxCategoryId.Text);
                    activatedcategoryinfo.ExpiryDate = DateTime.Parse("December 31, 9999");
                    var windowsAccountName = User.Identity.Name.Split('\\').Last();
                    activatedcategoryinfo.ModifiedBy = SecurityController.FindAccount(windowsAccountName).AccountID;

                    TextBoxCategoryId.Text = "";
                    TextBoxCategoryName.Text = "";
                    TextBoxCategoryDescription.Text = "";
                    TextBoxExpiryDate.Text = "";
                    TextBoxStartDate.Text = "";
                }

                var controller = new CategoryController();
                controller.ActivateCategory(activatedcategoryinfo);
                DropDownCategory.Items.Clear();
                DropDownCategory.DataBind();
                DropDownCategory.Items.Insert(0, new ListItem("Choose a Departrment", ""));
                DropDownCategory.SelectedIndex = -1;

                CheckboxExpired.Checked = false;
                ButtonActivateCategory.Visible = false;

            }, "Success", "Category : " + TextBoxCategoryName.Text + " has been activated.");

            ButtonCreateCategory.Visible = true;
            ButtonUpdateCategory.Visible = false;
            ButtonDeleteCategory.Visible = false;
            TextBoxCategoryName.Enabled = true;
            TextBoxCategoryDescription.Enabled = true;
            TextBoxExpiryDate.Enabled = true;

        }


    }
}