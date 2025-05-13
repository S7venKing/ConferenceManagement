import React, { useEffect, useState } from "react";
import Swal from "sweetalert2";
import { getRequest, postRequest } from "../../services/apiHelper";

const AdminNotifications = () => {
  const [message, setMessage] = useState("");
  const [searchDelegates, setSearchDelegates] = useState([]);
  const [delegateEmail, setDelegateEmail] = useState("");
  const [delegateId, setDelegateId] = useState(null);
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    fetchNotifications();
  }, []);

  const fetchNotifications = async () => {
    try {
      const response = await getRequest(`/notification/get-all-by-admin`);
      if (response.data != null) {
        setNotifications(response.data);
      }
    } catch (error) {
      Swal.fire("Error", error.message, "error");
    } finally {
      setLoading(false);
    }
  };
  const handleSearchDelegate = async () => {
    try {
      const response = await getRequest(
        `/delegates/get-by-email?email=${delegateEmail}`
      );
      setSearchDelegates(response.data || []);

      if (response.data.length <= 0) {
        Swal.fire({
          title: "Không tìm thấy đại biểu",
          text: "Nhập đúng email và thử lại",
          icon: "warning",
          confirmButtonText: "OK",
        });
      }
    } catch (error) {
      Swal.fire({
        title: "Không tìm thấy đại biểu",
        text: error.message,
        icon: "warning",
        confirmButtonText: "OK",
      });
    }
  };

  const sendNotificationToAll = async () => {
    if (!message) {
      Swal.fire("Error", "Message is required", "error");
      return;
    }

    try {
      await postRequest("/notification/send-to-all", { message });
      Swal.fire("Success", "Notification sent to all users", "success");
      setMessage("");
    } catch (error) {
      Swal.fire("Error", "Failed to send notification", "error");
    }
  };

  const sendNotificationToUser = async () => {
    if (!message || !delegateId) {
      Swal.fire("Error", "User and Message are required", "error");
      return;
    }

    try {
      await postRequest("/notification/send-to-user", {
        delegateId,
        message,
      });
      Swal.fire("Success", "Notification sent to user", "success");
      setMessage("");
      setDelegateId("");
    } catch (error) {
      Swal.fire("Error", "Failed to send notification", "error");
    }
  };

  return (
    <div className="p-4 max-w-lg mx-auto bg-white shadow-md rounded-lg">
      <h2 className="text-2xl font-bold mb-4">Send Notifications</h2>

      <textarea
        placeholder="Enter notification message"
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        className="w-full p-2 border rounded"
      />

      <button
        onClick={sendNotificationToAll}
        className="w-full mt-3 bg-blue-500 text-white p-2 rounded hover:bg-blue-600"
      >
        Send to All Users
      </button>

      <div className="mt-4">
        <label className="block mb-2">
          Email Đại Biểu
          <input
            type="email"
            value={delegateEmail}
            onChange={(e) => setDelegateEmail(e.target.value)}
            className="border p-2 w-full rounded"
          />
        </label>
        <button
          onClick={handleSearchDelegate}
          className="bg-blue-500 text-white px-4 py-2 rounded mb-4"
        >
          Tìm Đại Biểu
        </button>

        {searchDelegates.length > 0 && (
          <label className="block mb-2">
            Chọn Đại Biểu
            <select
              value={delegateId}
              onChange={(e) => setDelegateId(e.target.value)}
              className="border p-2 w-full rounded"
            >
              <option value="">Chọn đại biểu</option>
              {searchDelegates.map((delegate) => (
                <option key={delegate.id} value={delegate.id}>
                  {delegate.name} ({delegate.email})
                </option>
              ))}
            </select>
            <button
              onClick={sendNotificationToUser}
              className="w-full mt-2 bg-green-500 text-white p-2 rounded hover:bg-green-600"
            >
              Send to Selected User
            </button>
          </label>
        )}
      </div>
      <div className="p-4">
        <h2 className="text-2xl font-bold mb-4">Admin Notifications</h2>

        {loading ? (
          <p>Loading notifications...</p>
        ) : notifications.length === 0 ? (
          <p>No notifications available.</p>
        ) : (
          <div className="space-y-4">
            {notifications.map((notification) => (
              <div
                key={notification.id}
                className="p-4 bg-white shadow-md rounded-lg border"
              >
                <p className="font-semibold">{notification.message}</p>
                <p className="text-xs text-gray-500">
                  Created at:{" "}
                  {new Date(notification.createdAt).toLocaleString()}
                </p>
                <p className="text-xs text-gray-500">
                  Send To{" "}
                  {notification.userId != null
                    ? " UserId " + notification.userId
                    : "All"}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminNotifications;
