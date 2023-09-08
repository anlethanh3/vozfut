import { PropsWithChildren, ReactNode, useEffect, useState } from "react";
import { Container, Navbar, Nav, Button, Dropdown, Image, Row, Col, DropdownButton, NavItem } from "react-bootstrap";
import Login from "./Login";
import { ProfileProps, TokenProps, onProfile, onToken, selectProfile, signOut, } from '../slices/profileSlice';
import { useAppSelector, useAppDispatch } from '../app/hooks';
import { Link, useMatch, useResolvedPath } from "react-router-dom";
import { useCookies } from "react-cookie";
import axios from "axios";
export default function NavMenu() {
    const [isLogin, setIsLogin] = useState(false)
    const profile = useAppSelector(selectProfile)
    const dispatch = useAppDispatch()
    let key = { token: 'token', profile: 'profile' }
    const [cookies, setCookie, removeCookie] = useCookies([key.token, key.profile])

    useEffect(() => {
        if (cookies && cookies.token) {
            var token = cookies.token as TokenProps
            var profile = cookies.profile as ProfileProps
            axios.defaults.headers.common['Authorization'] = `${token.tokenType} ${token.accessToken}`
            dispatch(onToken(token))
            dispatch(onProfile(profile))
        }
    }, [])

    function imageUri(imageId: string) {
        return `https://drive.google.com/uc?export=view&id=${imageId}`
    }

    function showProfile() {
        return (
            <Row className="d-inline-block">
                <Col>
                    {
                        <Image className="px-0" style={{ width: '40px' }} src={imageUri(profile!.avatarUri)} roundedCircle />
                    }
                </Col>
            </Row>
        )
    }

    function onSignOut() {
        dispatch(signOut())
        removeCookie(key.token)
        removeCookie(key.profile)
        axios.defaults.headers.common['Authorization'] = ``
    }

    function Profile() {
        if (profile === undefined) {
            return (
                <Button onClick={() => setIsLogin(true)}>Login</Button>
            )
        }
        return (
            <DropdownButton flip id="dropdown-basic-button" title={showProfile()}>
                <Dropdown.Header>
                    <Image style={{ width: '100px' }} src={imageUri(profile.avatarUri)} roundedCircle />
                    <Dropdown.ItemText>Email: {profile.email}</Dropdown.ItemText>
                    <Dropdown.ItemText>Role: {profile.role}</Dropdown.ItemText>
                    <Dropdown.ItemText>Elo: +5</Dropdown.ItemText>
                </Dropdown.Header>
                <Dropdown.Divider />
                <Dropdown.Item>Specialist</Dropdown.Item>
                <Dropdown.Divider />
                <Dropdown.Item onClick={() => onSignOut()}>
                    Sign Out
                </Dropdown.Item>
            </DropdownButton >
        )
    }

    function Specialist() {
        return (
            <>
                <Dropdown.Item>Chỉ số chi tiết</Dropdown.Item>
                <Dropdown.ItemText>Giữ bóng: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Sút xa: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Dứt điểm: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Chuyền bóng: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Tốc độ chạy: 99</Dropdown.ItemText>
                <Dropdown.ItemText>Tăng tốc: 99</Dropdown.ItemText>
                <Dropdown.Divider />
                <Dropdown.ItemText>Chỉ số ẩn:</Dropdown.ItemText>
                <Dropdown.ItemText>5* Gáy</Dropdown.ItemText>
                <Dropdown.ItemText>5* Câu giờ</Dropdown.ItemText>
                <Dropdown.ItemText>Leadership</Dropdown.ItemText>
            </>
        )
    }
    interface CustomLinkProps {
        route: string,
    }
    function CustomLink(props: PropsWithChildren<CustomLinkProps>) {
        let { children, route } = props
        let resolved = useResolvedPath(route)
        let match = useMatch({ path: resolved.pathname, end: true })
        return (
            <NavItem>
                <Nav.Link style={{
                    fontWeight: match ? 'bold' : 'normal',
                    color: match ? 'var(--bs-warning)' : 'var(--bs-body-color)'
                }} as={Link} to={route} {...props} >{children}</Nav.Link>
            </NavItem>
        )
    }

    return (
        <>
            {
                isLogin &&
                <Login show={isLogin} onClose={() => setIsLogin(false)} onSubmit={(data) => {
                    setIsLogin(false)
                }} />
            }
            <Navbar expand="md" bg="primary" data-bs-theme="dark">
                <Container>
                    <Navbar.Brand href="/">
                        <img
                            alt=""
                            src="/icon.png"
                            width="30"
                            height="30"
                            className="d-inline-block align-top"
                        />{' '}Voz Football Club</Navbar.Brand>
                    <Navbar.Toggle />
                    <Navbar.Collapse >
                        <Nav className="me-auto">
                            <CustomLink route='/'>Home</CustomLink>
                            <CustomLink route="match">Match</CustomLink>
                            <CustomLink route="member">Member</CustomLink>
                            <CustomLink route="donate">Donate</CustomLink>
                            <CustomLink route="history">History</CustomLink>
                            <CustomLink route="counter">Counter</CustomLink>
                        </Nav>
                        <Nav>
                            <Profile />
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar >
        </>
    )
}