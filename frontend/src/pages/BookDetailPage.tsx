import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import { fetchBook, type Book } from '../api/books'
import { fetchUserBooks, updateUserBook, type UserBook } from '../api/userbooks'
import { useAuth } from '../AuthContext'
import bookcaseIcon from '../assets/icons/Icon_Bookcase.svg'
import checkIcon from '../assets/icons/Icon_Check.svg'
import arrowLeftIcon from '../assets/icons/Icon_Arrow_Left_Bold.svg'
import editIcon from '../assets/icons/Icon_Edit.svg'
import './BookDetailPage.css'

function formatDate(iso: string): string {
    return new Date(iso).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })
}

export default function BookDetailPage() {
    const { id } = useParams<{ id: string }>()
    const { token } = useAuth()
    const [book, setBook] = useState<Book | null>(null)
    const [error, setError] = useState<string | null>(null)
    const [userBook, setUserBook] = useState<UserBook | null>(null)
    const [showReadPicker, setShowReadPicker] = useState(false)
    const [selectedDate, setSelectedDate] = useState('')

    useEffect(() => {
        if (!id) return
        fetchBook(id).then(setBook).catch(() => setError('Could not load book'))
    }, [id])

    useEffect(() => {
        if (!token || !id) return
        fetchUserBooks(token)
            .then(ubs => setUserBook(ubs.find(ub => ub.bookId === id) ?? null))
            .catch(() => { })
    }, [token, id])

    async function toggleRead() {
        if (!token || !id) return
        if (userBook?.isRead) {
            const isOwned = userBook?.isOwned ?? false
            const updated: UserBook = { bookId: id, isRead: false, dateRead: null, isOwned }
            setUserBook(updated)
            await updateUserBook(id, { isRead: false, dateRead: null, isOwned }, token)
        } else {
            setSelectedDate('')
            setShowReadPicker(true)
        }
    }

    async function confirmRead() {
        if (!token || !id) return
        const dateRead = selectedDate ? new Date(selectedDate).toISOString() : null
        const isOwned = userBook?.isOwned ?? false
        const updated: UserBook = { bookId: id, isRead: true, dateRead, isOwned }
        setUserBook(updated)
        setShowReadPicker(false)
        await updateUserBook(id, { isRead: true, dateRead, isOwned }, token)
    }

    async function toggleOwned() {
        if (!token || !id) return
        const isOwned = !(userBook?.isOwned ?? false)
        const isRead = userBook?.isRead ?? false
        const dateRead = userBook?.dateRead ?? null
        const updated: UserBook = { bookId: id, isRead, dateRead, isOwned }
        setUserBook(updated)
        await updateUserBook(id, { isRead, dateRead, isOwned }, token)
    }

    if (error) return <p className="page-error">{error}</p>
    if (!book) return <p className="page-loading">Loading...</p>

    const isRead = userBook?.isRead ?? false
    const isOwned = userBook?.isOwned ?? false
    const heroColor = isRead && isOwned ? '#DDA5FD' : isRead ? '#CFA2FE' : isOwned ? '#B39CFE' : '#A599FF'
    const dateRead = userBook?.dateRead ?? null

    return (
        <div className="book-detail">
            <div className="book-detail-hero" style={{ backgroundColor: heroColor }}>
                <div className="book-detail-hero-nav">
                    <Link to="/"><img src={arrowLeftIcon} alt="Back" /></Link>
                    <Link to={`/books/${book.id}/edit`}><img src={editIcon} alt="Edit" /></Link>
                </div>
                <h1 className="book-detail-title">{book.title}</h1>
                {book.titleSwedish && <p className="book-detail-title-swedish">{book.titleSwedish}</p>}
            </div>
            <div className="book-detail-body">
                <p className="book-detail-meta">
                    {book.releaseYear} | {book.genre.name}
                    {book.detectives.length > 0 && ` | ${book.detectives.map(d => d.name).join(', ')}`}
                </p>
                <div className="book-detail-toggles">
                    <button className="book-detail-toggle" onClick={toggleRead}>
                        <span className="toggle-icon-box">
                            {isRead && <img src={checkIcon} alt="" className="toggle-icon-img" />}
                        </span>
                        <span>
                            {isRead && dateRead
                                ? `You read it on ${formatDate(dateRead)}`
                                : isRead
                                    ? 'You have read it'
                                    : 'Have you read this book?'
                            }
                        </span>
                    </button>
                    {showReadPicker && (
                        <div className="read-date-picker">
                            <input
                                type="date"
                                value={selectedDate}
                                onChange={e => setSelectedDate(e.target.value)}
                            />
                            <button onClick={confirmRead}>Mark as read</button>
                        </div>
                    )}
                    <button className="book-detail-toggle" onClick={toggleOwned}>
                        <span className="toggle-icon-box">
                            {isOwned && <img src={bookcaseIcon} alt="" className="toggle-icon-img" />}
                        </span>
                        <span>{isOwned ? 'You have it in your bookcase' : 'Do you own it?'}</span>
                    </button>
                </div>
                {book.synopsis && (
                    <section>
                        <h2>Synopsis</h2>
                        <p>{book.synopsis}</p>
                    </section>
                )}
                {book.trivia.length > 0 && (
                    <section>
                        <h2>Trivia</h2>
                        <ul className="trivia-list">
                            {book.trivia.map((item, i) => (
                                <li key={i} className="trivia-item-view">{item}</li>
                            ))}
                        </ul>
                    </section>
                )}
            </div>
        </div>
    )
}
