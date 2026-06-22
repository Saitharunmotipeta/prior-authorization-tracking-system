import axios from "axios";

export const specialistApiClient = axios.create({
  baseURL: "https://localhost:7225",
  headers: {
    "Content-Type": "application/json"
  }
});

export const payerApiClient = axios.create({
  baseURL: "https://localhost:7190",
  headers: {
    "Content-Type": "application/json"
  }
});

export const managerApiClient = axios.create({
  baseURL: "https://localhost:7273",
  headers: {
    "Content-Type": "application/json"
  }
});