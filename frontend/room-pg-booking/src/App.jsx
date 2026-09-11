import { useEffect, useState } from 'react'
import { Link, Route, Routes, useNavigate, useParams } from 'react-router-dom'
import { createBooking, getBookings, getRoom, getRooms, updateBookingStatus } from './api'

function Layout({ children }) {
  return <><nav><Link className="brand" to="/">StayEasy</Link><div><Link to="/">Home</Link><Link to="/rooms">Rooms</Link><Link to="/bookings">Bookings</Link><Link to="/admin">Admin</Link></div></nav>{children}<footer>© 2026 StayEasy Room & PG Booking</footer></>
}

function Home() {
  return <main className="hero"><div><span className="badge">ROOM & PG BOOKING</span><h1>Find a comfortable place to stay.</h1><p>Browse affordable rooms and PGs, check availability, and book your room online.</p><Link className="button" to="/rooms">Explore Rooms</Link></div></main>
}

function Rooms() {
  const [rooms, setRooms] = useState([])
  useEffect(() => { getRooms().then(r => setRooms(r.data)).catch(console.error) }, [])
  return <main className="container"><h2>Available Rooms</h2><div className="grid">{rooms.map(r => <div className="card" key={r.roomId}><div className="room-icon">🏠</div><h3>Room {r.roomNumber}</h3><p>{r.roomType} • {r.location}</p><strong>₹{r.rent.toLocaleString('en-IN')} / month</strong><p className="muted">{r.facilities}</p><span className={r.isAvailable ? 'available' : 'unavailable'}>{r.isAvailable ? 'Available' : 'Booked'}</span>{r.isAvailable && <Link className="button small" to={`/rooms/${r.roomId}`}>View & Book</Link>}</div>)}</div></main>
}

function RoomDetails() {
  const { id } = useParams()
  const [room, setRoom] = useState(null)
  const [form, setForm] = useState({ customerName:'', phone:'', email:'', moveInDate:'' })
  const [message, setMessage] = useState('')
  const navigate = useNavigate()
  useEffect(() => { getRoom(id).then(r => setRoom(r.data)).catch(() => setMessage('Room not found.')) }, [id])
  if (!room) return <main className="container"><p>{message || 'Loading...'}</p></main>
  const submit = async e => {
    e.preventDefault()
    try {
      await createBooking({ roomId: room.roomId, ...form })
      setMessage('Booking submitted successfully!')
      setTimeout(() => navigate('/bookings'), 800)
    } catch (err) { setMessage(err.response?.data || 'Booking failed.') }
  }
  return <main className="container two-col"><section className="card"><h2>Room {room.roomNumber}</h2><p><b>Type:</b> {room.roomType}</p><p><b>Location:</b> {room.location}</p><p><b>Rent:</b> ₹{room.rent.toLocaleString('en-IN')} / month</p><p><b>Facilities:</b> {room.facilities}</p></section><form className="card" onSubmit={submit}><h2>Book This Room</h2>{['customerName','phone','email','moveInDate'].map(k => <label key={k}>{k==='customerName'?'Name':k==='phone'?'Phone':k==='email'?'Email':'Move-in Date'}<input type={k==='moveInDate'?'date':k==='email'?'email':'text'} required value={form[k]} onChange={e=>setForm({...form,[k]:e.target.value})}/></label>)}<button className="button" type="submit">Confirm Booking</button>{message && <p className="message">{message}</p>}</form></main>
}

function Bookings() {
  const [bookings, setBookings] = useState([])
  useEffect(() => { getBookings().then(r=>setBookings(r.data)).catch(console.error) }, [])
  return <main className="container"><h2>Bookings</h2><div className="table-wrap"><table><thead><tr><th>Customer</th><th>Room</th><th>Move-in</th><th>Status</th></tr></thead><tbody>{bookings.map(b=><tr key={b.bookingId}><td>{b.customerName}<br/><small>{b.phone}</small></td><td>Room {b.room?.roomNumber}</td><td>{new Date(b.moveInDate).toLocaleDateString()}</td><td><span className="status">{b.status}</span></td></tr>)}</tbody></table></div></main>
}

function Admin() {
  const [bookings,setBookings]=useState([])
  const load=()=>getBookings().then(r=>setBookings(r.data))
  useEffect(()=>{load()},[])
  const change=async(id,status)=>{await updateBookingStatus(id,status);load()}
  return <main className="container"><h2>Admin Dashboard</h2><p className="muted">Simple booking management for the demo application.</p><div className="table-wrap"><table><thead><tr><th>ID</th><th>Customer</th><th>Room</th><th>Status</th><th>Action</th></tr></thead><tbody>{bookings.map(b=><tr key={b.bookingId}><td>#{b.bookingId}</td><td>{b.customerName}</td><td>{b.room?.roomNumber}</td><td>{b.status}</td><td><button onClick={()=>change(b.bookingId,'Approved')}>Approve</button> <button onClick={()=>change(b.bookingId,'Rejected')}>Reject</button></td></tr>)}</tbody></table></div></main>
}

export default function App() {
  return <Layout><Routes><Route path="/" element={<Home/>}/><Route path="/rooms" element={<Rooms/>}/><Route path="/rooms/:id" element={<RoomDetails/>}/><Route path="/bookings" element={<Bookings/>}/><Route path="/admin" element={<Admin/>}/></Routes></Layout>
}
