import React, { useEffect, useState } from "react";
import Swal from "sweetalert2";
import { getRequest, postRequest } from "../../services/apiHelper";

const Notifications = () => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    fetchNotifications();
  }, []);

  const fetchNotifications = async () => {
    try {
      const response = await getRequest(`/notification/get-all`);
      if (response.data != null) {
        setNotifications(response.data);
      }
    } catch (error) {
      Swal.fire("Error", error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-4 max-w-lg mx-auto bg-white shadow-md rounded-lg">
      <div className="p-4">
        <h2 className="text-2xl font-bold mb-4">Notifications</h2>

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
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Notifications;
