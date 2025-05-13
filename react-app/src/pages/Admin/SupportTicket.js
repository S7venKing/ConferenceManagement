import React, { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";

const SupportTickets = ({ isAdmin }) => {
  const [tickets, setTickets] = useState([]);
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const [replyContext, setReplyContext] = useState("");

  useEffect(() => {
    fetchTickets();
  }, []);

  const fetchTickets = async () => {
    try {
      const endpoint = isAdmin
        ? "/support-tickets/get-all"
        : "/support-tickets/get-by-me";
      const response = await getRequest(endpoint);
      setTickets(response.data);
    } catch (error) {
      console.error("Error fetching tickets", error);
    }
  };

  const submitTicket = async () => {
    if (!subject || !description) {
      Swal.fire("Error", "Subject and description are required", "error");
      return;
    }
    try {
      await postRequest("/support-tickets/create-tickets", {
        subject,
        description,
      });
      Swal.fire("Success", "Ticket created successfully", "success");
      setSubject("");
      setDescription("");
      fetchTickets();
    } catch (error) {
      Swal.fire("Error", "Failed to create ticket", "error");
    }
  };

  const resolveTicket = async (ticketId) => {
    const { value: replyContext } = await Swal.fire({
      title: "Reply to User",
      input: "textarea",
      inputPlaceholder: "Enter your response...",
      showCancelButton: true,
      confirmButtonText: "Submit",
      cancelButtonText: "Cancel",
    });

    if (!replyContext) {
      Swal.fire("Cancelled", "No response provided", "info");
      return;
    }

    try {
      await postRequest("/support-tickets/resolve-by-admin", {
        ticketId,
        replyContext,
      });
      Swal.fire("Success", "Ticket resolved successfully", "success");
      fetchTickets();
    } catch (error) {
      Swal.fire("Error", "Failed to resolve ticket", "error");
    }
  };

  const deleteTicket = async (id) => {
    try {
      await postRequest(`/support-tickets/delete-by-admin?id=${id}`);
      Swal.fire("Success", "Ticket deleted successfully", "success");
      fetchTickets();
    } catch (error) {
      Swal.fire("Error", "Failed to delete ticket", "error");
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto">
      {!isAdmin && (
        <div className="bg-white shadow-md rounded-lg p-6 mb-6">
          <h2 className="text-2xl font-bold mb-4">Submit a Support Ticket</h2>
          <input
            type="text"
            placeholder="Subject"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded-md mb-3"
          />
          <textarea
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="w-full p-2 border border-gray-300 rounded-md mb-3"
            rows="4"
          ></textarea>
          <button
            onClick={submitTicket}
            className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
          >
            Submit Ticket
          </button>
        </div>
      )}

      <h2 className="text-xl font-bold">
        {isAdmin ? "Admin Support Tickets" : "My Tickets"}
      </h2>
      {tickets.length > 0 &&
        tickets.map((ticket) => (
          <div
            key={ticket.id}
            className="bg-white shadow-md rounded-lg p-4 mt-2 flex flex-row justify-between"
          >
            <div>
              <p className="font-semibold">{ticket.subject}</p>
              <p className="text-sm text-gray-600">{ticket.description}</p>
              <p className="text-xs text-gray-500">User ID: {ticket.userId}</p>
              <p className="text-xs text-gray-500">
                Status: {ticket.isResolved ? "Resolved ✅" : "Pending ⏳"}
              </p>
            </div>

            <div>
              {isAdmin && !ticket.isResolved && (
                <button
                  onClick={() => resolveTicket(ticket.id)}
                  className="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600 mt-2"
                >
                  Resolve
                </button>
              )}

              {isAdmin && (
                <button
                  onClick={() => deleteTicket(ticket.id)}
                  className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600 mt-2 ml-2"
                >
                  Delete
                </button>
              )}
            </div>
          </div>
        ))}
    </div>
  );
};

export default SupportTickets;
