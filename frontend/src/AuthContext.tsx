import { createContext, useContext, useState, useEffect } from 'react'
import type { ReactNode } from 'react'
import { login as apiLogin, register as apiRegister } from './api/auth'
import type { AuthUser } from './api/auth'

interface AuthContextType {
    user: AuthUser | null
    token: string | null
    initializing: boolean
    login: (username: string, password: string) => Promise<string | null>
    register: (username: string, password: string) => Promise<string | null>
    logout: () => void
}

const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<AuthUser | null>(null)
    const [token, setToken] = useState<string | null>(null)
    const [initializing, setInitializing] = useState(true)

    useEffect(() => {
        const storedToken = localStorage.getItem('token')
        const storedUser = localStorage.getItem('user')
        if (storedToken && storedUser) {
            setToken(storedToken)
            setUser(JSON.parse(storedUser))
        }
        setInitializing(false)
    }, [])

    async function login(username: string, password: string): Promise<string | null> {
        const result = await apiLogin(username, password)
        if (result.success && result.user && result.token) {
            setUser(result.user)
            setToken(result.token)
            localStorage.setItem('token', result.token)
            localStorage.setItem('user', JSON.stringify(result.user))
            return null
        }
        return result.error ?? 'Login failed'
    }

    async function register(username: string, password: string): Promise<string | null> {
        const result = await apiRegister(username, password)
        if (result.success && result.user && result.token) {
            setUser(result.user)
            setToken(result.token)
            localStorage.setItem('token', result.token)
            localStorage.setItem('user', JSON.stringify(result.user))
            return null
        }
        return result.error ?? 'Registration failed'
    }

    function logout() {
        setUser(null)
        setToken(null)
        localStorage.removeItem('token')
        localStorage.removeItem('user')
    }

    return (
        <AuthContext.Provider value={{ user, token, initializing, login, register, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const ctx = useContext(AuthContext)
    if (!ctx) throw new Error('useAuth must be used within AuthProvider')
    return ctx
}
