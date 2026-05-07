const BASE = import.meta.env.VITE_API_BASE

export interface UserBook {
    bookId: string
    isRead: boolean
    dateRead: string | null
    isOwned: boolean
}

export async function fetchUserBooks(token: string): Promise<UserBook[]> {
    const res = await fetch(`${BASE}/api/userbooks`, {
        headers: { Authorization: `Bearer ${token}` }
    })
    if (!res.ok) throw new Error('Failed to fetch user books')
    return res.json()
}

export async function updateUserBook(
    bookId: string,
    data: { isRead: boolean; dateRead: string | null; isOwned: boolean },
    token: string
): Promise<void> {
    const res = await fetch(`${BASE}/api/userbooks/${bookId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    })
    if (!res.ok) throw new Error('Failed to update user book')
}
