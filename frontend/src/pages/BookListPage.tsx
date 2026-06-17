import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { fetchBooks, fetchGenres, type Book, type Genre, type Detective } from '../api/books'
import { fetchUserBooks, type UserBook } from '../api/userbooks'
import { useAuth } from '../AuthContext'
import cancelIcon from '../assets/icons/Icon_Cancel.svg'
import sortIcon from '../assets/icons/Icon_Sort.svg'
import arrowDownIcon from '../assets/icons/Icon_Arrow_Down.svg'
import arrowUpIcon from '../assets/icons/Icon_Arrow_Up.svg'
import bookcaseIcon from '../assets/icons/Icon_Bookcase.svg'
import searchIcon from '../assets/icons/Icon_Search.svg'
import './BookListPage.css'

function formatDate(iso: string): string {
    return new Date(iso).toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' })
}

export default function BookListPage() {
    const { token } = useAuth()
    const [books, setBooks] = useState<Book[]>([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [searchQuery, setSearchQuery] = useState('')
    const [genres, setGenres] = useState<Genre[]>([])
    const [userBookMap, setUserBookMap] = useState<Map<string, UserBook>>(new Map())
    const [selectedGenreId, setSelectedGenreId] = useState<string | null>(null)
    const [genreExpanded, setGenreExpanded] = useState(false)
    const [selectedDetectiveIds, setSelectedDetectiveIds] = useState<string[]>([])
    const [sortBy, setSortBy] = useState<'year' | 'alpha'>('year')
    const [sortDir, setSortDir] = useState<'asc' | 'desc'>('asc')
    const [sortExpanded, setSortExpanded] = useState(false)
    const [statusExpanded, setStatusExpanded] = useState(false)
    const [readFilter, setReadFilter] = useState<'read' | 'unread' | null>(null)
    const [ownedFilter, setOwnedFilter] = useState<'owned' | 'unowned' | null>(null)

    useEffect(() => {
        fetchBooks()
            .then(setBooks)
            .catch(() => setError('Could not load books'))
            .finally(() => setLoading(false))
    }, [])

    useEffect(() => {
        if (loading) return
        const saved = sessionStorage.getItem('bookListScrollY')
        if (saved) {
            window.scrollTo(0, parseInt(saved, 10))
            sessionStorage.removeItem('bookListScrollY')
        }
    }, [loading])

    useEffect(() => {
        fetchGenres().then(setGenres).catch(() => { })
    }, [])

    useEffect(() => {
        if (!token) return
        fetchUserBooks(token)
            .then(ubs => setUserBookMap(new Map(ubs.map(ub => [ub.bookId, ub]))))
            .catch(() => { })
    }, [token])

    if (loading) return <p className="page-loading">Loading...</p>
    if (error) return <p className="page-error">{error}</p>

    const genreFilteredBooks = selectedGenreId
        ? books.filter(b => b.genre.id === selectedGenreId)
        : books

    const displayedBooks = selectedDetectiveIds.length > 0
        ? genreFilteredBooks.filter(b =>
            selectedDetectiveIds.every(id => b.detectives.some(d => d.id === id))
        )
        : genreFilteredBooks

    const statusFilteredBooks = (readFilter !== null || ownedFilter !== null)
        ? displayedBooks.filter(b => {
            const ub = userBookMap.get(b.id)
            if (readFilter === 'read' && !(ub?.isRead ?? false)) return false
            if (readFilter === 'unread' && (ub?.isRead ?? false)) return false
            if (ownedFilter === 'owned' && !(ub?.isOwned ?? false)) return false
            if (ownedFilter === 'unowned' && (ub?.isOwned ?? false)) return false
            return true
        })
        : displayedBooks

    const dir = sortDir === 'asc' ? 1 : -1
    const sortedBooks = [...statusFilteredBooks].sort((a, b) =>
        sortBy === 'alpha'
            ? a.title.localeCompare(b.title) * dir
            : (a.releaseYear - b.releaseYear) * dir
    )

    const visibleBooks = searchQuery.trim()
        ? sortedBooks.filter(b =>
            b.title.toLowerCase().includes(searchQuery.toLowerCase()))
        : sortedBooks

    const availableDetectives: Detective[] = selectedGenreId
        ? Array.from(
            new Map(
                displayedBooks
                    .flatMap(b => b.detectives)
                    .filter(d => !selectedDetectiveIds.includes(d.id))
                    .map(d => [d.id, d])
            ).values()
        )
        : []

    function handleSortOption(option: 'year' | 'alpha') {
        if (sortBy === option) {
            setSortDir(d => d === 'asc' ? 'desc' : 'asc')
        } else {
            setSortBy(option)
            setSortDir('asc')
        }
        setSortExpanded(false)
    }

    function clearFilters() {
        setSelectedGenreId(null)
        setGenreExpanded(false)
        setSelectedDetectiveIds([])
    }

    function clearStatusFilters() {
        setReadFilter(null)
        setOwnedFilter(null)
        setStatusExpanded(false)
    }

    const readCount = [...userBookMap.values()].filter(ub => ub.isRead).length
    const ownedCount = [...userBookMap.values()].filter(ub => ub.isOwned).length
    const readPct = books.length > 0 ? Math.round((readCount / books.length) * 100) : 0
    const ownedPct = books.length > 0 ? Math.round((ownedCount / books.length) * 100) : 0

    return (
        <div className="book-list">
            <img src="/logo/Logo.svg" alt="Agatha Christie" className="book-list-logo" />
            <p className="book-stats">
                You have read <strong>{readPct}%</strong> of her books and own <strong>{ownedPct}%</strong> of them</p>

            
            <div className='search-bar'>
                <img src={searchIcon} alt="" className="search-icon" />
                <input 
                type='text'
                className='search-input'
                placeholder='Search for book...'
                value={searchQuery}
                onChange={e => setSearchQuery(e.target.value)}
                />
            </div>

            <div className="filter-bar">
                <div className="filter-row">
                    {(readFilter !== null || ownedFilter !== null || statusExpanded) && (
                        <button className="filter-clear" onClick={clearStatusFilters}>
                            <img src={cancelIcon} alt="Clear status filters" />
                        </button>
                    )}
                    {readFilter === null && ownedFilter === null ? (
                        <button
                            className={`filter-pill${statusExpanded ? ' filter-pill-active' : ''}`}
                            onClick={() => setStatusExpanded(e => !e)}
                        >
                            Status
                        </button>
                    ) : (
                        <button className="filter-pill filter-pill-l1" onClick={() => setStatusExpanded(true)}>Status</button>
                    )}
                    {readFilter !== null && (
                        <button className="filter-pill filter-pill-l2" onClick={() => { setReadFilter(null); setStatusExpanded(true) }}>
                            {readFilter === 'read' ? 'Read' : 'Unread'}
                        </button>
                    )}
                    {ownedFilter !== null && (
                        <button className={`filter-pill ${readFilter !== null ? 'filter-pill-l3' : 'filter-pill-l2'}`} onClick={() => { setOwnedFilter(null); setStatusExpanded(true) }}>
                            {ownedFilter === 'owned' ? 'Owns' : 'Unowned'}
                        </button>
                    )}
                    {readFilter === null && (statusExpanded || ownedFilter !== null) && (
                        <>
                            <button className="filter-pill" onClick={() => { setReadFilter('read'); setStatusExpanded(false) }}>Read</button>
                            <button className="filter-pill" onClick={() => { setReadFilter('unread'); setStatusExpanded(false) }}>Unread</button>
                        </>
                    )}
                    {ownedFilter === null && (statusExpanded || readFilter !== null) && (
                        <>
                            <button className="filter-pill" onClick={() => { setOwnedFilter('owned'); setStatusExpanded(false) }}>Owns</button>
                            <button className="filter-pill" onClick={() => { setOwnedFilter('unowned'); setStatusExpanded(false) }}>Unowned</button>
                        </>
                    )}
                </div>
                <div className="filter-row">
                    {(selectedGenreId || genreExpanded) && (
                        <button className="filter-clear" onClick={clearFilters}>
                            <img src={cancelIcon} alt="Clear filters" />
                        </button>
                    )}
                    {!selectedGenreId ? (
                        <button
                            className={`filter-pill${genreExpanded ? ' filter-pill-active' : ''}`}
                            onClick={() => setGenreExpanded(e => !e)}
                        >
                            Genre
                        </button>
                    ) : (
                        <>
                            <button className="filter-pill filter-pill-l1" onClick={() => { setSelectedGenreId(null); setSelectedDetectiveIds([]); setGenreExpanded(true) }}>Genre</button>
                            <button className="filter-pill filter-pill-l2" onClick={() => { setSelectedGenreId(null); setSelectedDetectiveIds([]); setGenreExpanded(true) }}>
                                {genres.find(g => g.id === selectedGenreId)?.name}
                            </button>
                        </>
                    )}
                    {genreExpanded && !selectedGenreId && genres.map(g => (
                        <button
                            key={g.id}
                            className="filter-pill"
                            onClick={() => { setSelectedGenreId(g.id); setGenreExpanded(false); }}>
                            {g.name}
                        </button>
                    ))}
                    {selectedGenreId && selectedDetectiveIds.map(detId => {
                        const det = books.flatMap(b => b.detectives).find(d => d.id === detId)
                        return det ? (
                            <button
                                key={detId}
                                className="filter-pill filter-pill-l3"
                                onClick={() => setSelectedDetectiveIds(ids => ids.filter(id => id !== detId))}>
                                {det.shortName ?? det.name}
                            </button>
                        ) : null
                    })}
                    {selectedGenreId && availableDetectives.map(d => (
                        <button
                            key={d.id}
                            className="filter-pill"
                            onClick={() => setSelectedDetectiveIds(ids => [...ids, d.id])}>
                            {d.shortName ?? d.name}
                        </button>
                    ))}
                </div>
                <div className="sort-trigger-row">
                    <button className="sort-trigger" onClick={() => setSortExpanded(e => !e)}>
                        <img src={sortIcon} alt="Sort" />
                        {sortBy === 'year' ? 'Year' : 'Alphabetical'}
                    </button>
                    {sortExpanded && (
                        <div className="sort-dropdown">
                            <p className="sort-heading">Sort by</p>
                            <button
                                className={`sort-option${sortBy === 'year' ? ' sort-option-active' : ''}`}
                                onClick={() => handleSortOption('year')}
                            >
                                Year
                                {sortBy === 'year' && (
                                    <img src={sortDir === 'asc' ? arrowDownIcon : arrowUpIcon} alt="" />
                                )}
                            </button>
                            <button
                                className={`sort-option${sortBy === 'alpha' ? ' sort-option-active' : ''}`}
                                onClick={() => handleSortOption('alpha')}
                            >
                                Alphabetical
                                {sortBy === 'alpha' && (
                                    <img src={sortDir === 'asc' ? arrowDownIcon : arrowUpIcon} alt="" />
                                )}
                            </button>
                        </div>
                    )}
                </div>
            </div>
            <ul className="book-grid">
                {visibleBooks.map(book => {
                    const ub = userBookMap.get(book.id)
                    const isOwned = ub?.isOwned ?? false
                    const isRead = ub?.isRead ?? false
                    const dateRead = ub?.dateRead ?? null
                    return (
                        <li
                            key={book.id}
                            className="book-card"
                            style={{ backgroundColor: isRead && isOwned ? '#DDA5FD' : isRead ? '#CFA2FE' : isOwned ? '#B39CFE' : '#A599FF' }}
                            onClick={() => sessionStorage.setItem('bookListScrollY', String(window.scrollY))}
                        >
                            <Link to={`/books/${book.id}`} className="book-card-link">
                                <div className="book-card-header">
                                    <span className="book-card-icon-box">
                                        {isOwned && <img src={bookcaseIcon} alt="Owned" className="book-card-icon-img" />}
                                    </span>
                                    <span className="book-card-status">
                                        {isRead && dateRead ? `Read on ${formatDate(dateRead)}` : isRead ? 'Read' : 'Unread'}
                                    </span>
                                </div>
                                <span className="book-card-title">{book.title}</span>
                                <span className="book-card-meta">
                                    {book.releaseYear} | {book.genre.name}
                                    {book.detectives.length > 0 && ` | ${book.detectives.map(d => d.name).join(', ')}`}
                                </span>
                            </Link>
                        </li>
                    )
                })}
            </ul>
        </div>
    )
}
