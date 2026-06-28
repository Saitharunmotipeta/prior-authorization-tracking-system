import axios from "axios";

export const specialistApiClient = axios.create({
  // baseURL: "http://localhost:5001",
  baseURL: "https://localhost:7225",
  headers: {
    "Content-Type": "application/json"
  }
});

export const payerApiClient = axios.create({
  // baseURL: "http://localhost:5003",
  baseURL: "https://localhost:7190",
  headers: {
    "Content-Type": "application/json"
  }
});

export const managerApiClient = axios.create({
  // baseURL: "http://localhost:5002",
  baseURL: "https://localhost:7273",
  headers: {
    "Content-Type": "application/json"
  }
});