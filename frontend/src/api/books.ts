export interface Detective {
    id: string
    name: string
    shortName: string | null
    hexColor: string
}

export interface Genre {
    id: string
    name: string
}

export interface MovieAdaptation {
    id: string
    title: string
    releaseYear: number
    posterImageUrl: string | null
}

export interface Book {
    id: string
    title: string
    titleSwedish: string | null
    releaseYear: number
    isShortStory: boolean
    synopsis: string | null
    trivia: string[]
    genre: Genre
    detectives: Detective[]
    movieAdaptations: MovieAdaptation[]
}

export interface BookRequest {
    title: string
    titleSwedish: string | null
    releaseYear: number
    isShortStory: boolean
    synopsis: string | null
    trivia: string[]
    genreId: string
    detectiveIds: string[]
}

const BASE = import.meta.env.VITE_API_BASE

export async function fetchBooks(): Promise<Book[]> {
    const res = await fetch(`${BASE}/books`)
    if (!res.ok) throw new Error('Failed to fetch books')
    return res.json()
}

export async function fetchBook(id: string): Promise<Book> {
    const res = await fetch(`${BASE}/books/${id}`)
    if (!res.ok) throw new Error('Failed to fetch book')
    return res.json()
}

export async function fetchGenres(): Promise<Genre[]> {
    const res = await fetch(`${BASE}/genres`)
    if (!res.ok) throw new Error('Failed to fetch genres')
    return res.json()
}

export async function fetchDetectives(): Promise<Detective[]> {
    const res = await fetch(`${BASE}/detectives`)
    if (!res.ok) throw new Error('Failed to fetch detectives')
    return res.json()
}

export async function updateBook(id: string, data: BookRequest):
    Promise<Book> {
    const res = await fetch(`${BASE}/books/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
    if (!res.ok) throw new Error('Failed to update book')
    return res.json()
}

export async function deleteBook(id: string): Promise<void> {
    const res = await fetch(`${BASE}/books/${id}`, { method: 'DELETE' })
    if (!res.ok) throw new Error('Failed to delete book')
}

export interface MovieAdaptationRequest {
    tmdbUrl: string
}

export async function addMovieAdaptation(
    bookId: string,
    data: MovieAdaptationRequest,
    token: string
): Promise<MovieAdaptation> {
    const res = await fetch(`${BASE}/books/${bookId}/movieadaptations`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    })
    if (!res.ok) throw new Error('Could not find that movie on TMDB')
    return res.json()
}