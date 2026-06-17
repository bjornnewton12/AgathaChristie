import { useState } from 'react'
import type { FormEvent } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../AuthContext'
import './AuthPage.css'

export default function RegisterPage() {
    const { register } = useAuth()
    const navigate = useNavigate()
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [repeatPassword, setRepeatPassword] = useState('')
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(false)

    async function handleSubmit(e: FormEvent) {
        e.preventDefault()
        setError(null)
        if (password !== repeatPassword) {
            setError('Passwords do not match')
            return
        }
        setLoading(true)
        try {
            const err = await register(username, password)
            if (err) {
                setError(err)
            } else {
                navigate('/')
            }
        } catch {
            setError('Could not connect to server')
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-page">
            <img src="/logo/Logo.svg" alt="Agatha Christie" className="auth-logo" />
            <form className="auth-card" onSubmit={handleSubmit}>
                <div className="auth-field">
                    <label htmlFor="username">Username</label>
                    <input
                        id="username"
                        type="text"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        required
                        autoComplete="username"
                    />
                </div>
                <div className="auth-field">
                    <label htmlFor="password">Password</label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        required
                        autoComplete="new-password"
                    />
                </div>
                <div className="auth-field">
                    <label htmlFor="repeat-password">Repeat password</label>
                    <input
                        id="repeat-password"
                        type="password"
                        value={repeatPassword}
                        onChange={e => setRepeatPassword(e.target.value)}
                        required
                        autoComplete="new-password"
                    />
                </div>
                {error && <p className="auth-error">{error}</p>}
                <button type="submit" className="auth-submit" disabled={loading}>
                    {loading ? 'Registering…' : 'Register'}
                </button>
                <Link to="/login" className="auth-link">Or go back to login</Link>
            </form>
        </div>
    )
}
