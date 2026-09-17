import axios from 'axios'

export const API_BASE = import.meta.env.VITE_API_URL || 'https://roompgbookingapi20260911152044-efb5b7b9bme0cqck.westus3-01.azurewebsites.net/api'

export const api = axios.create({ baseURL: API_BASE })

export const getRooms = () => api.get('/rooms')
export const getRoom = (id) => api.get(`/rooms/${id}`)
export const createBooking = (data) => api.post('/bookings', data)
export const getBookings = () => api.get('/bookings')
export const updateBookingStatus = (id, status) => api.put(`/bookings/${id}/status?status=${status}`)