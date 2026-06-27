import axios from "axios";

export const specialistApiClient = axios.create({
  baseURL: "http://localhost:5001",
  headers: {
    "Content-Type": "application/json"
  }
});

export const payerApiClient = axios.create({
  baseURL: "http://localhost:5003",
  headers: {
    "Content-Type": "application/json"
  }
});

export const managerApiClient = axios.create({
  baseURL: "http://localhost:5002",
  headers: {
    "Content-Type": "application/json"
  }
});