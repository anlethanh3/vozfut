import NavMenu from './NavMenu'
import { Outlet } from 'react-router-dom'
import { Container } from 'react-bootstrap';
function Layout() {
    return (
        <>
            <NavMenu />
            <Container>
                <Outlet />
            </Container>
        </>
    )
}
export default Layout;