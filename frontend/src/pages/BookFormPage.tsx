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
        trivia: [],
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

    if (error) return <p className="page-error">{error}</p>
    if (!base) return <p className="page-loading">Loading...</p>

    return (
        <div className="book-form">
            <h1 className="book-form-title">Edit Book</h1>
            <form onSubmit={handleSubmit} className="book-form-fields">
                <div className="book-form-field">
                    <label className="book-form-label">Detectives</label>
                    <div className="book-form-pills">
                        {detectives.map(d => (
                            <label
                                key={d.id}
                                className={`book-form-pill${form.detectiveIds.includes(d.id) ? ' selected' : ''}`}
                                style={form.detectiveIds.includes(d.id) ? { backgroundColor: d.hexColor, borderColor: d.hexColor } : {}}
                            >
                                <input
                                    type="checkbox"
                                    checked={form.detectiveIds.includes(d.id)}
                                    onChange={() => toggleDetective(d.id)}
                                />
                                {d.name}
                            </label>
                        ))}
                    </div>
                </div>
                <div className="book-form-field">
                    <h2>Synopsis</h2>
                    <textarea className="book-form-textarea" value={form.synopsis ?? ''} onChange={e => setForm(f =>
                        ({ ...f, synopsis: e.target.value || null }))} />
                </div>
                <div className="book-form-field">
                    <h2>Trivia</h2>
                    <textarea className="book-form-textarea" value={form.trivia.join('\n')} onChange={e => setForm(f =>
                        ({ ...f, trivia: e.target.value.split('\n').filter(l => l.trim()) }))} />
                </div>
                <div className="book-form-actions">
                    <button type="button" className="book-form-btn-cancel" onClick={() => navigate(`/books/${id}`)}>Cancel</button>
                    <button type="submit" className="book-form-btn-save">Save</button>
                </div>
            </form>
        </div>
    )
}