
using ChatApp.Constants;
using ChatApp.Hubs;
using ChatApp.Models;
using ChatApp.Models.DTOs;
using ChatApp.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppUnitTests.Services
{
    [TestFixture]
    class ChatHubServiceTests
    {
        private IEnumerable<ChatRoom> _repository = Database.Instance.ChatRooms;
        private ChatHubService _service;

        [SetUp]
        public void SetUp()
        {
            // Setting up signalR methods
            //
            //
            //

            Mock<IHubClients> mockClients = new Mock<IHubClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<IGroupManager> mockGroups = new Mock<IGroupManager>();

            var hubContext = new Mock<IHubContext<ChatHub>>();
            
            //SingalR mocks
            //--------------------------------------------------
            hubContext.Setup(context => context.Groups.AddToGroupAsync(It.IsIn<string>("connectionAdnrzejDefault"), It.IsIn<string>("Chat A"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.AddToGroupAsync(It.IsIn<string>("connectionBenekDefault"), It.IsIn<string>("Chat A"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.AddToGroupAsync(It.IsIn<string>("connectionKarolDefault"), It.IsIn<string>("Chat B"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.AddToGroupAsync(It.IsIn<string>("connectionRomanNew"), It.IsIn<string>("Chat A"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.RemoveFromGroupAsync(It.IsIn<string>("connectionAdnrzejDefault"), It.IsIn<string>("Chat A"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.RemoveFromGroupAsync(It.IsIn<string>("connectionBenekDefault"), It.IsIn<string>("Chat A"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Groups.RemoveFromGroupAsync(It.IsIn<string>("connectionKarolDefault"), It.IsIn<string>("Chat B"), new System.Threading.CancellationToken())).Returns(Task.FromResult(true));
            hubContext.Setup(context => context.Clients).Returns(() => mockClients.Object);

            //Setting up repository with example data
            //
            //
            //

            //Setting up service with mocked data and repository
            //--------------------------------------------------
            _service = new ChatHubService(hubContext.Object);
            var chatRooms = (List<ChatRoom>)_repository;

            //Chatroom "Chat A" with 2 users - Andrzej and Benek
            //--------------------------------------------------
            var chatRoom_ChatA = new ChatRoom();
            chatRoom_ChatA.ChatRoomName = "Chat A";
            chatRoom_ChatA.CreatedBy = "Andrzej";
            chatRoom_ChatA.ChatRoomId = Guid.NewGuid();

            var chatUser_Andrzej = new ChatUser();
            chatUser_Andrzej.ConnectionID = "connectionAdnrzejDefault";
            chatUser_Andrzej.UserName = "Andrzej";

            var chatUser_Benek = new ChatUser();
            chatUser_Benek.ConnectionID = "connectionBenekDefault";
            chatUser_Benek.UserName = "Benek";

            var userList_ChatA = (List<ChatUser>)chatRoom_ChatA.ChatUsers;
            userList_ChatA.Add(chatUser_Andrzej);
            userList_ChatA.Add(chatUser_Benek);

            chatRooms.Add(chatRoom_ChatA);

            //Chatroom "Chat B" with 1 users - Karol, chat created by Marek
            //-------------------------------------------------------------
            var chatRoom_ChatB = new ChatRoom();
            chatRoom_ChatB.ChatRoomName = "Chat B";
            chatRoom_ChatB.CreatedBy = "Marek";
            chatRoom_ChatB.ChatRoomId = Guid.NewGuid();

            var chatUser_Karol = new ChatUser();
            chatUser_Karol.ConnectionID = "connectionKarolDefault";
            chatUser_Karol.UserName = "Karol";

            var userList_ChatB = (List<ChatUser>)chatRoom_ChatB.ChatUsers;
            userList_ChatB.Add(chatUser_Karol);

            chatRooms.Add(chatRoom_ChatB);

        }

        [Test]
        public void UserIsGiven_And_ChatRoomIsEmpty_And_UserIsNotAssignedToAnyChat_Then_Return_USER_NOT_JOINED_TO_CHAT()
        {
            //GIVEN
            //
            //
            //
            var userDto = new ChatUserDTO();
            userDto.ChatRoomName = "";
            userDto.UserName = "Roman";
            var connectionId = "connectionRomanDefault";
            var correctResultDirective = new DirectiveDTO<string>(Commands.USER_NOT_JOINED_TO_CHAT, "User not joined to chat - not assigned to any chat or other error ocurs.");

            //WHEN
            //
            //
            //
            var result = _service.FindRoomForUserAsync(userDto, connectionId);

            //THEN
            //
            //
            //
            Assert.AreEqual(JsonConvert.SerializeObject(correctResultDirective), JsonConvert.SerializeObject(result.Result));
        }

        [Test]
        public void UserIsGiven_And_ChatRoomIsEmpty_And_UserIsAssignedToChat_Then_Return_USER_JOINED_EXIST_CHAT_And_UpdateConnectionId()
        {
            //GIVEN
            //
            //
            //
            var userDto = new ChatUserDTO();
            userDto.ChatRoomName = "";
            userDto.UserName = "Andrzej";
            var connectionId = "connectionAndrzejNew";

            var correctResultDirective = new DirectiveDTO<string>(Commands.USER_JOINED_EXIST_CHAT, "Chat A");

            //WHEN
            //
            //
            //
            var result = _service.FindRoomForUserAsync(userDto, connectionId);

            //THEN
            //
            //
            //

            //checking directive
            //------------------
            Assert.AreEqual(JsonConvert.SerializeObject(correctResultDirective), JsonConvert.SerializeObject(result.Result));
            //checking if connectionid is updated
            //------------------------------------
            Assert.AreEqual(connectionId, _repository.FirstOrDefault(room => room.ChatRoomName.Equals("Chat A")).ChatUsers.FirstOrDefault(user => user.UserName.Equals("Andrzej")).ConnectionID);
        }

        [Test]
        public void UserIsGiven_And_ChatRoomIsGiven_And_ChatRoomIsExists_And_UserIsNotAssignedToAnyChat_Then_Return_USER_JOINED_CHAT()
        {
            //GIVEN
            //
            //
            //
            var userDto = new ChatUserDTO();
            userDto.ChatRoomName = "Chat A";
            userDto.UserName = "Roman";
            var connectionId = "connectionRomanNew";

            var correctResultDirective = new DirectiveDTO<string>(Commands.USER_JOINED_CHAT, "Chat A");
            var chatUser = new ChatUser(userDto.UserName, connectionId);

            //WHEN
            //
            //
            //
            var result = _service.FindRoomForUserAsync(userDto, connectionId);

            //THEN
            //
            //
            //

            //checking directive
            //------------------
            Assert.AreEqual(JsonConvert.SerializeObject(correctResultDirective), JsonConvert.SerializeObject(result.Result));
            //checking if user is saved to repository
            //------------------------------------
            Assert.AreEqual(JsonConvert.SerializeObject(chatUser), JsonConvert.SerializeObject(_repository.FirstOrDefault(room => room.ChatRoomName.Equals("Chat A")).ChatUsers.FirstOrDefault(user => user.UserName.Equals("Roman"))));
        }
    }


}
