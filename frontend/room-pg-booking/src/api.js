import axios from 'axios'

export const API_BASE = import.meta.env.VITE_API_URL || 'https://localhost:7085/api'

export const api = axios.create({ baseURL: API_BASE })

export const getRooms = () => api.get('/rooms')
export const getRoom = (id) => api.get(`/rooms/${id}`)
export const createBooking = (data) => api.post('/bookings', data)
export const getBookings = () => api.get('/bookings')
export const updateBookingStatus = (id, status) => api.put(`/bookings/${id}/status?status=${status}`)
