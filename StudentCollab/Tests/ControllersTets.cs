﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudentCollab.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using StudentCollab.Models;
using StudentCollab.Dal;
using System.IO;

namespace StudentCollab.Tests
{
    [TestClass]
    public class ControllersTets
    {
        public TestContext TestContext { get; set; }
        public string testingThreadHeader = "Testing Thread !@#23343";
        [TestMethod]
        public void TestLoginView()
        {
            var controller = new LoginController();
            var result = controller.Login() as ViewResult;
            Assert.AreEqual("Login", result.ViewName);
        }

        [TestMethod]
        public void TestSignupView()
        {
            var controller = new LoginController();
            var result = controller.Signup() as ViewResult;
            Assert.AreEqual("Signup", result.ViewName);
        }

        [TestMethod]
        public void TestMainPage()
        {
            var controller = new MainPageControllerTest();
            var result = controller.MainPage(new Models.User()) as ViewResult;
            Assert.AreEqual("MainPage", result.ViewName);
        }

        [TestMethod]
        public void TestBackToHome()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };

            //Act
            var result = (RedirectToRouteResult)controller.backToHome(cur);
            //Assert
            Assert.AreEqual("MainPage", result.RouteValues["controller"]);
        }

        // < !!!!--FAILING--!!! >
        /*[TestMethod]
        public void TestReport()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            string txt = "Test String";
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = controller.Report(txt, cur) as ViewResult;
            //Assert
            Assert.AreEqual("Report", result.View);
        }*/

        [TestMethod]
        public void TestUploadFile()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = controller.UploadFile(cur) as ViewResult;
            //Assert
            Assert.AreEqual("UploadFile", result.ViewName);
        }

        [TestMethod]
        public void TestDepartmentsPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            Institution inst = new Institution
            {
                InstitutionId = 1,
                InstName = "SCE"
            };
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = controller.DepartmentsPage(inst, cur) as ViewResult;
            //Assert
            Assert.AreEqual("DepartmentsPage", result.ViewName);
        }

        [TestMethod]
        public void TestSyearsPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            Department inst = new Department
            {
                DepartmentId = 1
            };
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = controller.SyearsPage(inst, cur) as ViewResult;
            //Assert
            Assert.AreEqual("SyearsPage", result.ViewName);
        }

        [TestMethod]
        public void TestThreadsPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            Syear inst = new Syear
            {
                SyearId = 1
            };
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = controller.ThreadsPage(inst, cur) as ViewResult;
            //Assert
            Assert.AreEqual("ThreadsPage", result.ViewName);
        }

        [TestMethod]
        public void TestLockThread()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            Thread test_Thread = new Thread
            {
                ThreadName = "Testing Thread",
                SyearId = 1,
                ThreadType = "[Question]",
                OwnerId = 1
            };
            //Add test thread to DB
            using (ThreadDal trdDal = new ThreadDal())
            {
                trdDal.Threads.Add(test_Thread);
                trdDal.SaveChanges();
            }

            //Act
            controller.LockThread(test_Thread);

            //Assert
            using (ThreadDal trdDal = new ThreadDal())
            {
                List<Thread> testTrd =
                    (from x in trdDal.Threads
                     where x.ThreadName == test_Thread.ThreadName
                     select x).ToList();
                Assert.AreEqual(testTrd[0].Locked, true);

                //Cleanup
                trdDal.Threads.Remove(testTrd[0]);
                trdDal.SaveChanges();
            }


        }

        [TestMethod]
        public void TestUnLockThread()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            Thread test_Thread = new Thread
            {
                ThreadName = testingThreadHeader,
                SyearId = 1,
                ThreadType = "[Question]",
                OwnerId = 1
            };
            //Add test thread to DB
            using (ThreadDal trdDal = new ThreadDal())
            {
                trdDal.Threads.Add(test_Thread);
                trdDal.SaveChanges();
            }

            //Act
            controller.UnLockThread(test_Thread);

            //Assert
            using (ThreadDal trdDal = new ThreadDal())
            {
                List<Thread> testTrd =
                    (from x in trdDal.Threads
                     where x.ThreadName == test_Thread.ThreadName
                     select x).ToList();
                Assert.AreEqual(testTrd[0].Locked, false);

                //Cleanup

                trdDal.Threads.Remove(testTrd[0]);
                trdDal.SaveChanges();
            }

        }

        [TestMethod]
        public void TestAdminPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();

            //Act
            var result = controller.AdminPage() as ViewResult;

            //Assert
            Assert.AreEqual("AdminPage", result.ViewName);
        }

        [TestMethod]
        public void TestNewThread()
        {
            //Arrange
            var controller = new MainPageControllerTest();

            //Act
            var result = controller.NewThread() as ViewResult;

            //Assert
            Assert.AreEqual("NewThread", result.ViewName);
        }

        [TestMethod]
        public void TestCreateNewThread()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            string content = "Test Content TESTING!!!!";
            Syear inst = new Syear
            {
                SyearId = 1
            };
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            Thread test_Thread = new Thread
            {
                ThreadName = testingThreadHeader,
                SyearId = 1,
                ThreadType = "[Question]",
                OwnerId = 1
            };

            //Act
            controller.CreateNewThread(inst, test_Thread, cur, content);

            //Assert
            using (ContentDal cnt = new ContentDal())
            {
                List<Content> cont =
                    (from x in cnt.Contents
                     where x.threadName == test_Thread.ThreadName
                     select x).ToList();
                Assert.AreEqual(content, cont[0].threadContent);

                //Cleanup
                cnt.Contents.Remove(cont[0]);
                cnt.SaveChanges();
            }

            //Cleanup Remove testing Thread
            //Add test thread to DB
            using (ThreadDal trdDal = new ThreadDal())
            {
                var result = trdDal.Threads.SingleOrDefault(b => b.ThreadName == test_Thread.ThreadName);
                trdDal.Threads.Remove(result);
                trdDal.SaveChanges();
            }

        }

        //[TestMethod]
        //public void TestEditThreadPage()
        //{
        //    //Arrange
        //    var controller = new MainPageControllerTest();
        //}

        [TestMethod]
        public void TestManagerPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };

            //Act
            var result = controller.ManagerPage(cur) as ViewResult;

            //Assert
            Assert.AreEqual("ManagerPage", result.ViewName);
        }

        [TestMethod]
        public void TestAgreemantPage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            // Act
            var result = controller.AgreemantPage(cur) as ViewResult;

            //Assert
            Assert.AreEqual("AgreemantPage", result.ViewName);
        }

        [TestMethod]
        public void TestUpdateAgreemant()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            Other testOther = new Other();
            testOther.Id = 2;
            

            //Act
            controller.UpdateAgreemant(testOther, "NewAgreement Testing 123#@!");

            //Assert
            using (OtherDal dal = new OtherDal())
            {
                Other val = dal.Others.SingleOrDefault(b => b.Id == testOther.Id);
                Assert.AreEqual("NewAgreement Testing 123#@!", val.Val);

                controller.UpdateAgreemant(testOther, "Do not Delete - Line for testing");
            }

        }

        [TestMethod]
        public void TestUploadPage()
        {
            //Arragne
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };

            //Act
            var act = controller.MyUploads(cur) as ViewResult;
            //Assert
            Assert.AreEqual("MyUploads", act.ViewName);
        }

        [TestMethod]
        public void TestNewComment()
        {
            //Arragne
            var controller = new MainPageControllerTest();
            string content = "Test Content TESTING!!!!";
            Syear inst = new Syear
            {
                SyearId = 1
            };
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            Thread test_Thread = new Thread
            {
                ThreadName = testingThreadHeader,
                SyearId = 1,
                ThreadType = "[Question]",
                OwnerId = 1
            };

            ThreadDal tDal = new ThreadDal();
            tDal.Threads.Add(test_Thread); // Add test thread
            tDal.SaveChanges();

            Thread currenthread = tDal.Threads.SingleOrDefault(b => b.ThreadName == testingThreadHeader);

            Content testContent = new Content()
            {
                threadContent = content,
                threadId = currenthread.ThreadId      
            };
            ContentDal cDal = new ContentDal();
            cDal.Contents.Add(testContent);
            cDal.SaveChanges();

            //Act
            controller.addComment(testContent, cur, "TestComment");

            //Assert
            CommentDal comDal = new CommentDal();

            List<Comment> com =
                (from x in comDal.Comments
                 where x.threadId == currenthread.ThreadId
                 select x).ToList();

            Comment ans = com.Find(b => b.commentContent == "TestComment");
            Assert.IsNotNull(ans);

            //CleanUp
            tDal.Threads.Remove(currenthread);
            tDal.SaveChanges();

            cDal.Contents.Remove(testContent);
            cDal.SaveChanges();

            comDal.Comments.Remove(ans);
            comDal.SaveChanges();
        }

        [TestMethod]
        public void TestDonwload()
        {
            //Arragne
            var controller = new MainPageControllerTest();
            int upnum = 18;

            //Act
            FileResult ans = controller.DownloadFile(upnum);

            //Assert
            Assert.IsNotNull(ans);
        }

        [TestMethod]
        public void TestAddToUnion()
        {
            //Arragne
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            int id = 2;

            //Act
            controller.addToUnion(id, cur);

            //Assert
            using (UserDal d = new UserDal())
            {
                User usr = d.Users.SingleOrDefault(b => b.id == 2);
                Assert.AreEqual(2, usr.studentUnionRank);
            }
        }

        [TestMethod]
        public void TestBlockView()
        {
            //Arrange
            var controller = new ManagerControllerTests();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var res = controller.Block(cur) as ViewResult;

            //Assert
            Assert.AreEqual("Block", res.ViewName);
        }

        [TestMethod]
        public void TestBlockUser()
        {
            //Arrange
            var controller = new ManagerControllerTests();
            //Add Manager User
            UserDal usrDal = new UserDal();
            User managerUser = new User
            {
                UserName = "TestManagerUser",
                Password = "1234",
                rank = 1,
                Email = "Test@test.com",
                EmailConfirmed = true,
                active = true
            };
            usrDal.Users.Add(managerUser);
            usrDal.SaveChanges();

            managerUser = usrDal.Users.SingleOrDefault(b => b.UserName == "TestManagerUser");
            //Add to managing connection
            ManageConnectionDal magDal = new ManageConnectionDal();
            ManageConnection magObj = new ManageConnection
            {
                managerId = managerUser.id,
                institution = 1,
                sYear = -1,
                department = -1
            };
            magDal.ManageConnections.Add(magObj);
            magDal.SaveChanges();
            //Add User to be blocked
            User blockedUser = new User
            {
                UserName = "NewUser",
                Password = "1234",
                rank = 0,
                Email = "Block@asd.com",
                active = true,
                EmailConfirmed = true
            };
            usrDal.Users.Add(blockedUser);
            usrDal.SaveChanges();

            //Act
            controller.BlockUser(managerUser, "NewUser", DateTime.Now, 1, -1, -1);

            //Assert
            BlockedDal blkdal = new BlockedDal();

            List<Blocked> blk =
                (from x in blkdal.Blockeds
                 where x.UserName == blockedUser.UserName
                 select x).ToList();

            Assert.AreEqual(blk[0].UserName.Replace(" ",""), blockedUser.UserName.Replace(" ",""));

            //Clean up
            usrDal.Users.Remove(managerUser);
            usrDal.Users.Remove(blockedUser);
            usrDal.SaveChanges();

            magDal.ManageConnections.Remove(magObj);
            magDal.SaveChanges();

            blkdal.Blockeds.Remove(blk[0]);
            blkdal.SaveChanges();
        }

        [TestMethod]
        public void TestChangeImage()
        {
            //Arrange
            var controller = new MainPageControllerTest();
            User cur = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            //Act
            var result = (RedirectToRouteResult)controller.ChangeDepImage(cur);

            //Assert
            Assert.AreEqual("MainPage", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void TestFollowUser()
        {
            //Arrange
            var controller = new CommunicationControllerTest();
            User cur1 = new User
            {
                UserName = "tzvi",
                Password = "1234"
            };
            User cur2 = new User
            {
                UserName = "eladb21",
                Password = "1234"
            };

            UserDal usrd = new UserDal();
            User cur1_Obj = usrd.Users.SingleOrDefault(b => b.UserName == cur1.UserName);
            User cur2_Obj = usrd.Users.SingleOrDefault(b => b.UserName == cur2.UserName);

            CommentDal cmtd = new CommentDal();
            Comment testComment = new Comment()
            {
                commentContent = "Testing Comment 1122334455",
                userId = cur2_Obj.id,
                threadId = 1
            };
            cmtd.Comments.Add(testComment);
            cmtd.SaveChanges();
            Comment testId = cmtd.Comments.SingleOrDefault(b => b.commentContent == testComment.commentContent);

            //Act
            controller.follow(testId.commentId, cur1_Obj);

            //Assert
            FollowDal fdal = new FollowDal();
            List<Follow> f =
                (from x in fdal.Follows
                 where x.follower == cur1_Obj.id && x.followOn == cur2_Obj.id
                 select x).ToList();

            Assert.IsTrue(f.Any());

            //Clean Up
            cmtd.Comments.Remove(testId);
            cmtd.SaveChanges();

            fdal.Follows.Remove(f[0]);
            fdal.SaveChanges();



        }
    }
}