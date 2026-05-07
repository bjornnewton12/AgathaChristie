import { Navigate, Outlet } from 'react-router-dom'
import { useAuth } from '../AuthContext'

export default function ProtectedRoute() {
    const { user, initializing } = useAuth()
    if (initializing) return null
    return user ? <Outlet /> : <Navigate to="/login" replace />
}
