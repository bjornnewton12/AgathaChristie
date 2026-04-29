import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import {
    fetchBook,
    fetchDetectives,
    updateBook,
    type Detective,
    type BookRequest
} from '../api/books'
import './BookFormPage.css'

export default function BookFormPage() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()

    const [detectives, setDetectives] = useState<Detective[]>([])
    const [error, setError] = useState<string | null>(null)
    const [form, setForm] = useState<Pick<BookRequest, 'synopsis' | 'trivia' | 'detectiveIds'>>({
        synopsis: null,
        trivia: null,
        detectiveIds: []
    })
    const [base, setBase] = useState<BookRequest | null>(null)

    useEffect(() => {
        if (!id) return
        Promise.all([fetchBook(id), fetchDetectives()])
            .then(([book, dets]) => {
                setDetectives(dets)
                const full: BookRequest = {
                    title: book.title,
                    titleSwedish: book.titleSwedish,
                    releaseYear: book.releaseYear,
                    isShortStory: book.isShortStory,
                    genreId: book.genre.id,
                    synopsis: book.synopsis,
                    trivia: book.trivia,
                    detectiveIds: book.detectives.map(d => d.id)
                }
                setBase(full)
                setForm({ synopsis: book.synopsis, trivia: book.trivia, detectiveIds: book.detectives.map(d => d.id) })
            })
            .catch(() => setError('Could not load book'))
    }, [id])

    function toggleDetective(detectiveId: string) {
        setForm(f => ({
            ...f,
            detectiveIds: f.detectiveIds.includes(detectiveId)
                ? f.detectiveIds.filter(d => d !== detectiveId)
                : [...f.detectiveIds, detectiveId]
        }))
    }

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault()
        if (!id || !base) return
        try {
            await updateBook(id, { ...base, ...form })
            navigate(`/books/${id}`)
        } catch {
            setError('Could not save book')
        }
    }

    if (error) return <p>{error}</p>
    if (!base) return <p>Loading...</p>

    return (
        <div>
            <h1>Edit Book</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Detectives</label>
                    {detectives.map(d => (
                        <label key={d.id}>
                            <input
                                type="checkbox"
                                checked={form.detectiveIds.includes(d.id)}
                                onChange={() => toggleDetective(d.id)}
                            />
                            {d.name}
                        </label>
                    ))}
                </div>
                <div>
                    <label>Synopsis</label>
                    <textarea value={form.synopsis ?? ''} onChange={e => setForm(f =>
                        ({ ...f, synopsis: e.target.value || null }))} />
                </div>
                <div>
                    <label>Trivia</label>
                    <textarea value={form.trivia ?? ''} onChange={e => setForm(f =>
                        ({ ...f, trivia: e.target.value || null }))} />
                </div>
                <button type="button" onClick={() => navigate(`/books/${id}`)}>Cancel</button>
                <button type="submit">Save</button>
            </form>
        </div>
    )
}