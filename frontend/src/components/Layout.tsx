import { useState, useRef, useEffect } from 'react'
import { Outlet, Link, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../AuthContext'
import profileIcon from '../assets/icons/Icon_Profile.svg'
import './Layout.css'

export default function Layout() {
    const { user, logout } = useAuth()
    const navigate = useNavigate()
    const location = useLocation()
    const isBookDetail = /^\/books\/[^/]+$/.test(location.pathname)
    const [profileOpen, setProfileOpen] = useState(false)
    const profileRef = useRef<HTMLDivElement>(null)

    useEffect(() => {
        function handleClickOutside(e: MouseEvent) {
            if (profileRef.current && !profileRef.current.contains(e.target as Node)) {
                setProfileOpen(false)
            }
        }
        document.addEventListener('mousedown', handleClickOutside)
        return () => document.removeEventListener('mousedown', handleClickOutside)
    }, [])

    function handleLogout() {
        logout()
        setProfileOpen(false)
        navigate('/login')
    }

    return (
        <div className="layout">
            {!isBookDetail && <div className="layout-auth">
                {user ? (
                    <div className="profile-menu" ref={profileRef}>
                        <button className="profile-btn" onClick={() => setProfileOpen(o => !o)}>
                            <img src={profileIcon} alt="Profile" />
                        </button>
                        {profileOpen && (
                            <div className="profile-dropdown">
                                <span className="profile-dropdown-username">{user.username}</span>
                                <button className="profile-dropdown-logout" onClick={handleLogout}>Logout</button>
                            </div>
                        )}
                    </div>
                ) : (
                    <Link to="/login" className="layout-auth-btn">Login</Link>
                )}
            </div>}
            <main className="main">
                <Outlet />
            </main>
        </div>
    )
}
