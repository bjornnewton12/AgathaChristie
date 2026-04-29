import { Link } from 'react-router-dom'
import { Outlet } from 'react-router-dom'
import './Layout.css'

export default function Layout() {
    return (
        <div>
            <header>
                <Link to="/">
                    <img src="/src/assets/icons/Icon_Bookcase.svg" alt="Home" />
                    <span>Agatha Christie Books</span>
                </Link>
            </header>
            <main>
                <Outlet />
            </main>
        </div>
    )
}