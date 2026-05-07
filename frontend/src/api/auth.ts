const BASE = import.meta.env.VITE_API_BASE

export interface AuthUser {
    id: string
    username: string
    createdAt: string
}

interface AuthResponse {
    success: boolean
    user: AuthUser | null
    token: string | null
    error: string | null
}

export async function login(username: string, password: string): Promise<AuthResponse> {
    const res = await fetch(`${BASE}/api/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    })
    if (res.ok) return res.json()
    const error = await res.text()
    return { success: false, user: null, token: null, error }
}

export async function register(username: string, password: string): Promise<AuthResponse> {
    const res = await fetch(`${BASE}/api/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    })
    if (res.ok) return res.json()
    const error = await res.text()
    return { success: false, user: null, token: null, error }
}

export async function checkUsername(username: string): Promise<boolean> {
    const res = await fetch(`${BASE}/api/auth/check-username/${encodeURIComponent(username)}`)
    if (!res.ok) return false
    const data = await res.json()
    return data.exists
}
